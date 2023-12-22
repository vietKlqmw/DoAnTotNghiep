------------------------------------------------Material------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_SEARCH
    @p_MaterialCode NVARCHAR(40),
    @p_MaterialGroup NVARCHAR(9)
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.StorageLocation, mm.ProductionType, mm.StandardPrice, 
           mm.MovingPrice, mm.MaterialOrigin, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE (@p_MaterialCode IS NULL OR mm.MaterialCode LIKE CONCAT('%', @p_MaterialCode, '%'))
       AND (@p_MaterialGroup IS NULL OR mm.MaterialGroup LIKE CONCAT('%', @p_MaterialGroup, '%'))
       AND mm.IsDeleted = 0
END
------------------------------------------------Get data by Id:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_BY_ID
    @p_MaterialId BIGINT
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.StorageLocation, mm.ProductionType, mm.StandardPrice, 
           mm.MovingPrice, mm.MaterialOrigin, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE mm.Id = @p_MaterialId
END
------------------------------------------------GetListMasterialUsage:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_MATERIAL_USAGE
AS
BEGIN
    SELECT mm.Id MaterialId, mm.MaterialType + ' - ' + mm.MaterialCode MaterialCode
      FROM MasterMaterial mm 
     WHERE ((mm.EffectiveDateTo IS NULL AND mm.EffectiveDateFrom <= GETDATE()) OR mm.EffectiveDateTo >= GETDATE())
       AND mm.IsDeleted = 0
END
------------------------------------------------Edit data:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_EDIT
@p_MaterialId INT, 
@p_MaterialType NVARCHAR(4),
@p_MaterialCode NVARCHAR(40), 
@p_Description NVARCHAR(40), 
@p_MaterialGroup NVARCHAR(9), 
@p_BaseUnitOfMeasure NVARCHAR(3), 
@p_StorageLocation NVARCHAR(4), 
@p_ProductionType NVARCHAR(10), 
@p_StandardPrice DECIMAL, 
@p_MovingPrice DECIMAL, 
@p_MaterialOrigin NVARCHAR(1), 
@p_EffectiveDateFrom DATE, 
@p_EffectiveDateTo DATE, 
@p_UserId BIGINT
AS
BEGIN
    IF @p_MaterialId IS NULL
    BEGIN
        INSERT INTO MasterMaterial (CreationTime, CreatorUserId, IsDeleted, 
                                    MaterialType, MaterialCode, Description, MaterialGroup, BaseUnitOfMeasure, 
                                    StorageLocation, StandardPrice, MovingPrice, MaterialOrigin, EffectiveDateFrom, 
                                    EffectiveDateTo, ProductionType)
                            VALUES (GETDATE(), @p_UserId, 0, 
                                    @p_MaterialType, @p_MaterialCode, @p_Description, @p_MaterialGroup, @p_BaseUnitOfMeasure, 
                                    @p_StorageLocation, @p_StandardPrice, @p_MovingPrice, @p_MaterialOrigin, @p_EffectiveDateFrom, 
                                    @p_EffectiveDateTo, @p_ProductionType);
    END
    ELSE
    BEGIN
        UPDATE MasterMaterial
           SET Description = @p_Description,
               BaseUnitOfMeasure = @p_BaseUnitOfMeasure,
               StandardPrice = @p_StandardPrice,
               MovingPrice = @p_MovingPrice,
               EffectiveDateFrom = @p_EffectiveDateFrom,
               EffectiveDateTo = @p_EffectiveDateTo,
               LastModificationTime = GETDATE(),
               LastModifierUserId = @p_UserId
         WHERE Id = @p_MaterialId
    END

END
------------------------------------------------Delete data:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_DELETE
    @p_Id INT
AS
BEGIN
    UPDATE MasterMaterial
       SET IsDeleted = 1
     WHERE Id = @p_Id 
END
------------------------------------------------Import data:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_MERGE
    @Guid VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY 
	  BEGIN TRANSACTION

	  -- FOR BUG CHECK-
		INSERT INTO ProcessLog(CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('MasterMaterial', 'MasterMaterial Import', 'START:', 'SYSTEM', GETDATE());

		IF NOT EXISTS (SELECT 1 FROM MasterMaterial_T mmt WHERE mmt.Guid = @Guid AND mmt.ErrorDescription != '')
		BEGIN					
		    MERGE INTO MasterMaterial AS P
		    USING (
        	  SELECT DISTINCT t.MaterialType, t.MaterialCode, t.Description, t.MaterialGroup, t.BaseUnitOfMeasure, 
                   t.StorageLocation, t.StandardPrice, t.MovingPrice, t.MaterialOrigin, t.EffectiveDateFrom, 
                   t.EffectiveDateTo, t.ProductionType, t.CreatorUserId
        		  FROM MasterMaterial_T t
        	   WHERE Guid = @Guid
		    ) AS DT	ON (P.MaterialType = DT.MaterialType AND P.MaterialCode = DT.MaterialCode)				    
		    WHEN MATCHED THEN
		        UPDATE SET 
					      P.Description = DT.Description,
					      P.MaterialGroup = DT.MaterialGroup,
					      P.BaseUnitOfMeasure = DT.BaseUnitOfMeasure,
					      P.StorageLocation = DT.StorageLocation,
					      P.StandardPrice = DT.StandardPrice,
                P.MovingPrice = DT.MovingPrice,
                P.MaterialOrigin = DT.MaterialOrigin,
                P.EffectiveDateFrom = DT.EffectiveDateFrom,
                P.EffectiveDateTo = DT.EffectiveDateTo,
                P.ProductionType = DT.ProductionType,
                P.LastModifierUserId = DT.CreatorUserId,
					      P.LastModificationTime = GETDATE()
		    WHEN NOT MATCHED THEN
		        INSERT (CreationTime, CreatorUserId, IsDeleted, 
                    MaterialType, MaterialCode, Description, MaterialGroup, BaseUnitOfMeasure, 
                    StorageLocation, StandardPrice, MovingPrice, MaterialOrigin, EffectiveDateFrom, 
                    EffectiveDateTo, ProductionType)				
		  	    VALUES (GETDATE(), DT.CreatorUserId, 0, 
                    DT.MaterialType, DT.MaterialCode, DT.Description, DT.MaterialGroup, DT.BaseUnitOfMeasure, 
                    DT.StorageLocation, DT.StandardPrice, DT.MovingPrice, DT.MaterialOrigin, DT.EffectiveDateFrom, 
                    DT.EffectiveDateTo, DT.ProductionType);

		END

		-- FOR BUG CHECK-
		INSERT INTO ProcessLog (CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('MasterMaterial', 'MasterMaterial Import', 'END:', 'SYSTEM', GETDATE());

		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorMessage NVARCHAR(4000);
  
		SELECT @ErrorMessage = ERROR_MESSAGE(),
			     @ErrorSeverity = ERROR_SEVERITY(), 
			     @ErrorState = ERROR_STATE();

		ROLLBACK TRANSACTION		
		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
		VALUES ('MasterMaterial', 'MasterMaterial Import', 'ERROR :' + @ErrorMessage + 
															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
	   
	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	  END CATCH;
END
GO
------------------------------------------------Get List Import:
CREATE OR ALTER PROCEDURE INV_MASTER_MATERIAL_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(max)
AS 
BEGIN
    SELECT DISTINCT mmt.Guid, mmt.MaterialType, mmt.MaterialCode, mmt.Description, mmt.MaterialGroup, 
           mmt.BaseUnitOfMeasure, mmt.StorageLocation, mmt.ProductionType, mmt.StandardPrice, mmt.MovingPrice, 
           mmt.MaterialOrigin, mmt.EffectiveDateFrom, mmt.EffectiveDateTo, mmt.ErrorDescription
      FROM MasterMaterial_T mmt
     WHERE mmt.Guid = @Guid AND ISNULL(mmt.ErrorDescription, '') <> '' 
END
------------------------------------------------PartList------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_MASTER_PART_LIST_SEARCH
(
    @p_PartNo NVARCHAR(50),
    @p_SupplierNo NVARCHAR(10),
    @p_Cfc NVARCHAR(4)
)
AS
BEGIN
    SELECT mpl.Id, mpl.PartNo, mpl.PartName, mpl.SupplierNo, mpl.SupplierId, mpl.MaterialId, 
           mpl.CarfamilyCode, mpl.StartProductionMonth, mpl.EndProductionMonth, mpl.Remark
      FROM MasterPartList mpl
     WHERE (@p_PartNo IS NULL OR mpl.PartNo LIKE CONCAT('%', @p_PartNo, '%'))
       AND (@p_SupplierNo IS NULL OR mpl.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_Cfc IS NULL OR mpl.CarfamilyCode LIKE CONCAT('%', @p_Cfc, '%'))
       AND mpl.IsDeleted = 0
END
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_MASTER_PART_LIST_EDIT
(
    @p_PartListId INT,
    @p_PartNo NVARCHAR(15),
    @p_PartName NVARCHAR(500),
    @p_SupplierNo NVARCHAR(10),
    @p_Cfc NVARCHAR(4),
    @p_Remark NVARCHAR(MAX),
    @p_StartProductionMonth DATE,
    @p_EndProductionMonth DATE,
    @p_MaterialId INT,
    @p_UserId BIGINT
)
AS
BEGIN
    DECLARE @SupplierId INT = (SELECT Id FROM MasterSupplierList WHERE SupplierNo = @p_SupplierNo);
    IF @p_PartListId IS NULL
    BEGIN
        INSERT INTO MasterPartList 
                    (CreationTime, CreatorUserId, IsDeleted, 
                    PartNo, PartName, SupplierNo, SupplierId, MaterialId, CarfamilyCode, 
                    StartProductionMonth, EndProductionMonth, Remark)
             VALUES (GETDATE(), @p_UserId, 0, 
                    UPPER(@p_PartNo), @p_PartName, @p_SupplierNo, @SupplierId, @p_MaterialId, @p_Cfc, 
                    @p_StartProductionMonth, @p_EndProductionMonth, @p_Remark);
    END
    ELSE
    BEGIN
        UPDATE MasterPartList 
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId, 
               PartNo = UPPER(@p_PartNo), 
               PartName = @p_PartName, 
               SupplierNo = @p_SupplierNo, 
               SupplierId = @SupplierId, 
               MaterialId = @p_MaterialId, 
               CarfamilyCode = @p_Cfc, 
               StartProductionMonth = @p_StartProductionMonth, 
               EndProductionMonth = @p_EndProductionMonth, 
               Remark = @p_Remark
         WHERE Id = @p_PartListId;
    END
END
------------------------------------------------Import:
CREATE OR ALTER PROCEDURE INV_MASTER_PART_LIST_MERGE
    @Guid VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY 
	  BEGIN TRANSACTION

	  -- FOR BUG CHECK-
		INSERT INTO ProcessLog(CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('MasterPartList', 'MasterPartList Import', 'START:', 'SYSTEM', GETDATE());

    UPDATE t1 
		   SET ErrorDescription = CONCAT(ErrorDescription, N' Material không tồn tại hoặc chưa có hiệu lực!')
		  FROM MasterPartList_T t1
		 WHERE Guid = @Guid 
		   AND t1.PartNo IS NOT NULL
       AND t1.PartName IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM MasterMaterial t2 
                        WHERE t2.MaterialCode = SUBSTRING(t1.PartNo, 1, 10) AND t2.Description = t1.PartName
                          AND ((t2.EffectiveDateTo IS NULL AND t2.EffectiveDateFrom <= GETDATE()) OR t2.EffectiveDateTo >= GETDATE())
                          AND t1.StartProductionMonth > t2.EffectiveDateFrom
                          AND t2.IsDeleted = 0);

    UPDATE t1 
		   SET ErrorDescription = CONCAT(ErrorDescription, N' Không tồn tại SupplierNo: ', t1.SupplierNo)
		  FROM MasterPartList_T t1
		 WHERE Guid = @Guid 
		   AND t1.SupplierNo IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM MasterSupplierList t2 WHERE t2.SupplierNo = t1.SupplierNo AND t2.IsDeleted = 0);

     UPDATE t1 
		   SET ErrorDescription = CONCAT(ErrorDescription, N' Không tồn tại CarfamilyCode: ', t1.CarfamilyCode)
		  FROM MasterPartList_T t1
		 WHERE Guid = @Guid 
		   AND t1.CarfamilyCode IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM MasterCarfamily t2 WHERE t2.Code = t1.CarfamilyCode AND t2.IsDeleted = 0);

		IF NOT EXISTS (SELECT 1 FROM MasterPartList_T WHERE Guid = @Guid AND ErrorDescription != '')
		BEGIN					
		    MERGE INTO MasterPartList AS P
		    USING (
        	  SELECT DISTINCT t.PartNo, t.PartName, t.SupplierNo, t.CarfamilyCode, t.StartProductionMonth, 
                   t.EndProductionMonth, t.Remark, t.CreatorUserId, msl.Id SupplierId, mm.Id MaterialId
        		  FROM MasterPartList_T t
         LEFT JOIN MasterSupplierList msl ON t.SupplierNo = msl.SupplierNo
         LEFT JOIN MasterMaterial mm ON SUBSTRING(t.PartNo, 1, 10) = mm.MaterialCode AND t.PartName = mm.Description
        	   WHERE t.Guid = @Guid
		    ) AS DT	ON (P.CarfamilyCode = DT.CarfamilyCode AND P.SupplierNo = DT.SupplierNo AND P.PartNo = DT.PartNo AND P.PartName = DT.PartName)				    
		    WHEN MATCHED THEN
		        UPDATE SET 
					      P.StartProductionMonth = DT.StartProductionMonth,
					      P.EndProductionMonth = DT.EndProductionMonth,
					      P.Remark = DT.Remark,
                P.LastModifierUserId = DT.CreatorUserId,
					      P.LastModificationTime = GETDATE()
		    WHEN NOT MATCHED THEN
		        INSERT (CreationTime, CreatorUserId, IsDeleted, 
                    PartNo, PartName, SupplierNo, SupplierId, MaterialId, CarfamilyCode, 
                    StartProductionMonth, EndProductionMonth, Remark)				
		  	    VALUES (GETDATE(), DT.CreatorUserId, 0, 
                    DT.PartNo, DT.PartName, DT.SupplierNo, DT.SupplierId, DT.MaterialId, DT.CarfamilyCode,  
                    DT.StartProductionMonth, DT.EndProductionMonth, DT.Remark);

		END

		-- FOR BUG CHECK-
		INSERT INTO ProcessLog (CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('MasterPartList', 'MasterPartList Import', 'END:', 'SYSTEM', GETDATE());

		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorMessage NVARCHAR(4000);
  
		SELECT @ErrorMessage = ERROR_MESSAGE(),
			     @ErrorSeverity = ERROR_SEVERITY(), 
			     @ErrorState = ERROR_STATE();

		ROLLBACK TRANSACTION		
		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
		VALUES ('MasterPartList', 'MasterPartList Import', 'ERROR :' + @ErrorMessage + 
															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
	   
	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	  END CATCH;
END
------------------------------------------------Get List Error Import:
CREATE OR ALTER PROCEDURE INV_MASTER_PART_LIST_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(max)
AS 
BEGIN
    SELECT DISTINCT mplt.Guid, mplt.PartNo, mplt.PartName, mplt.SupplierNo, 
           mplt.CarfamilyCode, mplt.Remark, mplt.StartProductionMonth, 
           mplt.EndProductionMonth, mplt.ErrorDescription
      FROM MasterPartList_T mplt
     WHERE mplt.Guid = @Guid AND ISNULL(mplt.ErrorDescription, '') <> '' 
END
------------------------------------------------Get List:
CREATE OR ALTER PROCEDURE INV_PROD_OTHER_GET_LIST_PART_FOR_ORDER
AS
BEGIN
    SELECT mpl.PartNo, mpl.SupplierNo, mpl.CarfamilyCode, mpl.PartName, 
           mm.StandardPrice, mm.BaseUnitOfMeasure, mm.Id MaterialId 
      FROM MasterPartList mpl
 LEFT JOIN MasterMaterial mm on mpl.MaterialId = mm.Id
     WHERE mpl.IsDeleted = 0
END
------------------------------------------------MaterialGroup------------------------------------------------
INSERT INTO MasterMaterialGroup 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'VEH0001', N'Vehicle'),
(GETDATE(), 1, 0, N'UNT0001', N'Engine'),
(GETDATE(), 1, 0, N'UNT0002', N'Transmission'),
(GETDATE(), 1, 0, N'UNT0003', N'Axles'),
(GETDATE(), 1, 0, N'UNT0004', N'Propeller Shaft'),
(GETDATE(), 1, 0, N'PAR0001', N'Part'),
(GETDATE(), 1, 0, N'DRM0001', N'Direct Material');
------------------------------------------------MaterialType------------------------------------------------
INSERT INTO MasterMaterialType 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'ZDRM', N'Direct Material'),
(GETDATE(), 1, 0, N'ZPAR', N'EG,T/M,Axles,Shaft,Part'),
(GETDATE(), 1, 0, N'ZVEH', N'Vehicle');
------------------------------------------------ProductType------------------------------------------------
INSERT INTO MasterProductType 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'VEH', N'Vehicle'),
(GETDATE(), 1, 0, N'UNT', N'Unit ( Engine, T/M, Axles, Propeller Shaft)'),
(GETDATE(), 1, 0, N'PAR', N'Part'),
(GETDATE(), 1, 0, N'DRM', N'Direct'),
(GETDATE(), 1, 0, N'SUP', N'Supplies'),
(GETDATE(), 1, 0, N'NSM', N'Non-stock Material');
------------------------------------------------StorageLocation------------------------------------------------
INSERT INTO MasterStorageLocation 
(CreationTime, CreatorUserId, IsDeleted, 
StorageLocation, StorageLocationName, Category,
AddressLanguageEn, AddressLanguageVn)
VALUES 
(GETDATE(), 1, 0, 
'A1', 'GWH (CKD part)', 'Raw Material',
'Tran Thanh Ngo Ward - Hai Phong City - Viet Nam', 
N'Phường Trần Thành Ngọ - Thành phố Hải Phòng - Việt Nam'),
(GETDATE(), 1, 0, 
'A2', 'GWH (CKD part)', 'Raw Material',
'Phuc Thang Ward - Phuc Yen City - Vinh Phuc Province - Viet Nam', 
N'Phường Phúc Thắng - Thành phố Phúc Yên - Tỉnh Vĩnh Phúc - Việt Nam'),
(GETDATE(), 1, 0, 
'B1', 'GWH (CKD part)', 'Raw Material',
'An Hai Bac Ward - Da Nang City - Viet Nam', 
N'Phường An Hải Bắc - Thành phố Đà Nẵng - Việt Nam'),
(GETDATE(), 1, 0, 
'C1', 'GWH (CKD part)', 'Raw Material',
'Thanh My Loi Ward - HCM City - Viet Nam', 
N'Phường Thạnh Mỹ Lợi - Thành phố HCM - Việt Nam'),
(GETDATE(), 1, 0, 
'C2', 'GWH (CKD part)', 'Raw Material',
'Thu Duc District - Thu Duc City - Viet Nam', 
N'Quận Thủ Đức - Thành phố Thủ Đức - Việt Nam');
--Search
CREATE OR ALTER PROCEDURE INV_MASTER_STORAGE_LOCATION_SEARCH
    @p_PlantName NVARCHAR(30),
    @p_StorageLocationName NVARCHAR(MAX),
    @p_AddressLanguageEn NVARCHAR(MAX),
    @p_Category NVARCHAR(50)
AS 
BEGIN
    SELECT msl.Id, msl.StorageLocation, msl.StorageLocationName, 
           msl.AddressLanguageEn, msl.AddressLanguageVn, msl.Category
      FROM MasterStorageLocation msl
     WHERE (@p_StorageLocationName IS NULL OR msl.StorageLocationName LIKE CONCAT('%', @p_StorageLocationName, '%'))
       AND (@p_AddressLanguageEn IS NULL OR msl.AddressLanguageEn LIKE CONCAT('%', @p_AddressLanguageEn, '%'))
       AND (@p_Category IS NULL OR msl.Category LIKE CONCAT('%', @p_Category, '%'))
       AND msl.IsDeleted = 0
END
------------------------------------------------GetList:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_WAREHOUSE
AS
BEGIN
    SELECT msl.StorageLocation, msl.AddressLanguageVn 
      FROM MasterStorageLocation msl 
     WHERE msl.IsDeleted = 0
END
------------------------------------------------InvoiceStatus------------------------------------------------
INSERT INTO MasterInvoiceStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description)
VALUES 
(GETDATE(), 1, 0, N'IS01', N'NEW'),
(GETDATE(), 1, 0, N'IS02', N'PRE CUSTOMS'),
(GETDATE(), 1, 0, N'IS03', N'CUSTOMS DECLARED');
------------------------------------------------CustomsStatus------------------------------------------------
INSERT INTO MasterCustomsStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description)
VALUES 
(GETDATE(), 1, 0, N'CuS1', N'NEW'),
(GETDATE(), 1, 0, N'CuS2', N'NOT PAID (REQUESTED)'),
(GETDATE(), 1, 0, N'CuS3', N'PAID');
------------------------------------------------ContainerStatus------------------------------------------------
INSERT INTO MasterContainerStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description, DescriptionVn)
VALUES 
(GETDATE(), 1, 0, N'CS01', N'ON SEA', N'TRÊN BIỂN'),
(GETDATE(), 1, 0, N'CS02', N'PORT/ARRIVED', N'Ở CẢNG'),
(GETDATE(), 1, 0, N'CS03', N'TRANSFERRING', N'ĐANG ĐI ĐƯỜNG'),
(GETDATE(), 1, 0, N'CS04', N'YARD (WAITING)', N'BÃI CHỜ'),
(GETDATE(), 1, 0, N'CS05', N'DEVANNING', N'ĐÃ DỠ'),
(GETDATE(), 1, 0, N'CS06', N'RETURN CASE', N'ĐÓNG VỎ THÙNG HÀNG'),
(GETDATE(), 1, 0, N'CS07', N'EXPORT', N'XUẤT KHẨU'),
(GETDATE(), 1, 0, N'CS08', N'NEW', N'MỚI');
------------------------------------------------GetList:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_CONTAINER_STATUS
AS
BEGIN
    SELECT mcs.Code, mcs.Description 
      FROM MasterContainerStatus mcs
END
------------------------------------------------SupplierList------------------------------------------------
INSERT INTO MasterSupplierList
(CreationTime, CreatorUserId, IsDeleted, SupplierNo, SupplierName, SupplierType, SupplierNameVn)
VALUES 
(GETDATE(), 1, 0, 'STM', 'STM', 'ITNL', 'STM'),
(GETDATE(), 1, 0, 'TMC', 'Japan', 'ITNL', 'Japan'),
(GETDATE(), 1, 0, 'CTF', 'CHIEN TAI FONG', 'ITNL', 'CHIEN TAI FONG'),
(GETDATE(), 1, 0, 'EIDAIKAKO', 'EIDAIKAKO', 'LCL', 'EIDAIKAKO'),
(GETDATE(), 1, 0, 'TAM', 'Indonesia(Tam)', 'ITNL', 'Indonesia(Tam)'),
(GETDATE(), 1, 0, 'TMI', 'Indonesia(TMI)', 'ITNL', 'Indonesia(TMI)'),
(GETDATE(), 1, 0, 'UMW', 'Malaysia', 'ITNL', 'Malaysia'),
(GETDATE(), 1, 0, 'T&K', 'Malysia(T&K)', 'ITNL', 'Malysia(T&K)'),
(GETDATE(), 1, 0, 'TAP', 'Philipine(TAP)', 'ITNL', 'Philipine(TAP)'),
(GETDATE(), 1, 0, 'TMP', 'Philipine(TMP)', 'ITNL', 'Philipine(TMP)'),
(GETDATE(), 1, 0, 'SINGAPORE', 'SINGAPORE', 'ITNL', 'SINGAPORE'),
(GETDATE(), 1, 0, 'KUO', 'Taiwan(kuo)', 'ITNL', 'Taiwan(kuo)'),
(GETDATE(), 1, 0, 'TMT', 'Thailand(TMT)', 'ITNL', 'Thailand(TMT)'),
(GETDATE(), 1, 0, 'USA', 'USA', 'ITNL', 'USA'),
(GETDATE(), 1, 0, 'AUS', 'AUSTRALIA', 'ITNL', 'AUSTRALIA'),
(GETDATE(), 1, 0, 'USA', 'USA', 'ITNL', 'USA'),
(GETDATE(), 1, 0, 'GTE', 'GTE', 'ITNL', N'GTE(Trung Quốc)'),
(GETDATE(), 1, 0, 'TLI', 'Philipine(TMP)', 'ITNL', 'Philipine(TMP)');
------------------------------------------------GetSupplierList:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_SUPPLIER
AS
BEGIN
    SELECT msl.SupplierNo, msl.SupplierName 
      FROM MasterSupplierList msl
END
------------------------------------------------Forwarder------------------------------------------------
INSERT INTO MasterForwarder 
(CreationTime, CreatorUserId, IsDeleted, Code, Name, SupplierId)
VALUES 
(GETDATE(), 1, 0, 'NYK', 'YLSV', 6),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 6),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 2),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 2),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 2),
(GETDATE(), 1, 0, 'VMDC', 'VIMADECO', 2),
(GETDATE(), 1, 0, 'VMDC', 'VIMADECO', 13),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 13),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 13),
(GETDATE(), 1, 0, 'NAMPHAT', 'NAMPHAT', 15),
(GETDATE(), 1, 0, 'EGLV', 'EGLV', 16),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 16),
(GETDATE(), 1, 0, 'NIPPONHN', 'NIPPON HN', 17),
(GETDATE(), 1, 0, 'SAFIHN', 'SAFI HN', 18),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 18),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 7),
(GETDATE(), 1, 0, 'VMDC', 'VIMADECO', 7),
(GETDATE(), 1, 0, 'YLSV', 'Yusen Logistics', 10),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 10),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 1),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 9),
(GETDATE(), 1, 0, 'NYK', 'YLSV', 12);
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_MASTER_FORWARDER_SEARCH
(
    @p_Code NVARCHAR(10),
    @p_Namw NVARCHAR(100),
    @p_SupplierNo NVARCHAR(10)
)
AS
BEGIN
    SELECT mf.Id, mf.Code, mf.Name, mf.SupplierId, msl.SupplierNo 
      FROM MasterForwarder mf
 LEFT JOIN MasterSupplierList msl ON mf.SupplierId = msl.Id
     WHERE (@p_Code IS NULL OR mf.Code LIKE CONCAT('%', @p_Code, '%'))
       AND (@p_Namw IS NULL OR mf.Name LIKE CONCAT('%', @p_Namw, '%'))
       AND (@p_SupplierNo IS NULL OR msl.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND mf.IsDeleted = 0;
END 
------------------------------------------------GetList:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_FORWARDER_BY_SUPPLIERID
    @p_SupplierNo NVARCHAR(10)
AS
BEGIN
    DECLARE @p_SupplierId INT = (SELECT Id FROM MasterSupplierList WHERE SupplierNo = @p_SupplierNo);
    SELECT mf.Code, mf.Name 
      FROM MasterForwarder mf
     WHERE mf.SupplierId = @p_SupplierId;
END
------------------------------------------------UnitOfMeasure------------------------------------------------
INSERT INTO MasterUnitOfMeasure 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'PC', N'Pieces'),
(GETDATE(), 1, 0, N'EA', N'Each'),
(GETDATE(), 1, 0, N'RM', N'Ream'),
(GETDATE(), 1, 0, N'CAN', N'Can'),
(GETDATE(), 1, 0, N'L', N'Liter'),
(GETDATE(), 1, 0, N'ML', N'Milliliter'),
(GETDATE(), 1, 0, N'ROL', N'Roll'),
(GETDATE(), 1, 0, N'BT', N'Bottle'),
(GETDATE(), 1, 0, N'DR', N'Drum'),
(GETDATE(), 1, 0, N'GAL', N'Gallon'),
(GETDATE(), 1, 0, N'TB', N'Tube'),
(GETDATE(), 1, 0, N'TN', N'Tin'),
(GETDATE(), 1, 0, N'PAA', N'Pair'),
(GETDATE(), 1, 0, N'PAL', N'Pallet'),
(GETDATE(), 1, 0, N'PL', N'Pail'),
(GETDATE(), 1, 0, N'AU', N'Activ.unit'),
(GETDATE(), 1, 0, N'BAG', N'Bag'),
(GETDATE(), 1, 0, N'DZ', N'Dozen'),
(GETDATE(), 1, 0, N'SH', N'Sheet'),
(GETDATE(), 1, 0, N'CYL', N'Cylinder'),
(GETDATE(), 1, 0, N'MON', N'Months'),
(GETDATE(), 1, 0, N'D', N'Days'),
(GETDATE(), 1, 0, N'H', N'Hour'),
(GETDATE(), 1, 0, N'MIN', N'min.'),
(GETDATE(), 1, 0, N'MON', N'Months'),
(GETDATE(), 1, 0, N'PDA', N'Consultant Days'),
(GETDATE(), 1, 0, N'S', N'Second'),
(GETDATE(), 1, 0, N'UNT', N'Unit'),
(GETDATE(), 1, 0, N'LOT', N'Lot'),
(GETDATE(), 1, 0, N'CV', N'Case');
------------------------------------------------Carfamily------------------------------------------------
INSERT INTO MasterCarfamily 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, '565W', '565W'),
(GETDATE(), 1, 0, '578W', '578W'),
(GETDATE(), 1, 0, '665W', '665W'),
(GETDATE(), 1, 0, '681W', '681W'),
(GETDATE(), 1, 0, '694W', '694W'),
(GETDATE(), 1, 0, '718W', '718W'),
(GETDATE(), 1, 0, '728W', '728W'),
(GETDATE(), 1, 0, '757W', '757W'),
(GETDATE(), 1, 0, '764W', '764W'),
(GETDATE(), 1, 0, '815W', '815W'),
(GETDATE(), 1, 0, '824W', '824W'),
(GETDATE(), 1, 0, 'N/A', 'Not Applicable'),
(GETDATE(), 1, 0, '758W', '758W'),
(GETDATE(), 1, 0, '785W', '785W'),
(GETDATE(), 1, 0, 'D33H', 'D33H'),
(GETDATE(), 1, 0, 'D30H', 'D30H'),
(GETDATE(), 1, 0, '874W', '874W'),
(GETDATE(), 1, 0, '889W', '889W'),
(GETDATE(), 1, 0, 'D48H', 'D48H'),
(GETDATE(), 1, 0, 'D55L', 'D55L'),
(GETDATE(), 1, 0, 'D80N', 'D80N'),
(GETDATE(), 1, 0, 'D14N', 'D14N'),
(GETDATE(), 1, 0, 'D64G', 'D64G'),
(GETDATE(), 1, 0, '579W', '579W'),
(GETDATE(), 1, 0, '624W', '624W'),
(GETDATE(), 1, 0, 'D46H', 'D46H'),
(GETDATE(), 1, 0, 'D09H', 'D09H'),
(GETDATE(), 1, 0, 'D23H', 'D23H'),
(GETDATE(), 1, 0, 'D26H', 'D26H'),
(GETDATE(), 1, 0, 'D31H', 'D31H');
------------------------------------------------GetList:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_CFC
AS
BEGIN
    SELECT mc.Code, mc.Name 
      FROM MasterCarfamily mc
END
------------------------------------------------Shipment------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_PROD_SHIPMENT_SEARCH
(
    @p_ShipmentNo NVARCHAR(20),
    @p_SupplierNo NVARCHAR(10),
    @p_FromPort NVARCHAR(50),
    @p_ToPort NVARCHAR(50),
    @p_ShipmentDateFrom DATE,
    @p_ShipmentDateTo DATE,
    @p_Status NVARCHAR(50)
)
AS 
    SELECT DISTINCT a.Id, a.ShipmentNo, a.SupplierNo, a.Forwarder,
           a.Buyer, a.FromPort, a.ToPort, a.ShipmentDate,
           a.Etd, a.Eta, a.Ata, a.OceanVesselName, a.Atd, a.Status, 
           (CASE WHEN pci.Id IS NULL THEN 1 ELSE 0 END) IsEmptyShipment
      FROM ProdShipment a
 LEFT JOIN ProdContainerIntransit pci ON a.Id = pci.ShipmentId
     WHERE (@p_ShipmentNo IS NULL OR a.ShipmentNo LIKE CONCAT('%', @p_ShipmentNo, '%'))
       AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_FromPort IS NULL OR a.FromPort LIKE CONCAT('%', @p_FromPort, '%'))
       AND (@p_ToPort IS NULL OR a.ToPort LIKE CONCAT('%', @p_ToPort, '%'))
       AND (@p_ShipmentDateFrom IS NULL OR a.ShipmentDate >= @p_ShipmentDateFrom)
       AND (@p_ShipmentDateTo IS NULL OR a.ShipmentDate <= @p_ShipmentDateTo)
       AND (@p_Status IS NULL OR a.Status = @p_Status)
       AND a.IsDeleted = 0
  ORDER BY a.Status , a.ShipmentDate DESC 
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_PROD_SHIPMENT_EDIT
(
    @p_ShipmentId INT,
    @p_ShipmentNo NVARCHAR(20),
    @p_SupplierNo NVARCHAR(10),
    @p_FromPort NVARCHAR(50),
    @p_ToPort NVARCHAR(50),
    @p_Forwarder NVARCHAR(10),
    @p_Etd DATE,
    @p_Eta DATE,
    @p_OceanVesselName NVARCHAR(30),
    @p_ShipmentDate DATE,
    @p_Status NVARCHAR(50),
    @p_ListCont NVARCHAR(MAX),
    @p_UserId BIGINT
)
AS
BEGIN
    IF @p_ShipmentId IS NOT NULL
    BEGIN
        UPDATE ProdShipment 
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId, 
               FromPort = @p_FromPort, 
               ToPort = @p_ToPort, 
               ShipmentDate = @p_ShipmentDate, 
               Etd = @p_Etd, 
               Eta = @p_Eta,
               OceanVesselName = @p_OceanVesselName,
               Status = @p_Status
         WHERE Id = @p_ShipmentId;
    END
    ELSE
    BEGIN
        INSERT INTO ProdShipment (CreationTime, CreatorUserId, IsDeleted, 
                                  ShipmentNo, SupplierNo, FromPort, ToPort, Status, 
                                  Forwarder, Etd, Eta, OceanVesselName, ShipmentDate)
                          VALUES (GETDATE(), @p_UserId, 0, 
                                  @p_ShipmentNo, @p_SupplierNo, @p_FromPort, @p_ToPort, @p_Status, 
                                  @p_Forwarder, @p_Etd, @p_Eta, @p_OceanVesselName, @p_ShipmentDate);

        DECLARE @ShipId BIGINT = (SELECT Id FROM ProdShipment WHERE ShipmentNo = @p_ShipmentNo);

        UPDATE ProdContainerIntransit
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               ShipmentId = @ShipId,
               ShippingDate = @p_ShipmentDate
         WHERE Id IN (SELECT item FROM dbo.fnSplit(@p_ListCont, ','))
    END

    IF @p_Status = 'ORDERED'
    BEGIN
        IF @p_ShipmentId IS NULL 
        BEGIN
            SET @p_ShipmentId = (SELECT Id FROM ProdShipment WHERE ShipmentNo = @p_ShipmentNo);
        END

        DECLARE @p_BillofladingNo NVARCHAR(20) = 'BOL/' + FORMAT(GETDATE(), 'yyMMddHHmmss');
        IF EXISTS (SELECT 1 FROM ProdBillOfLading WHERE BillofladingNo = @p_BillofladingNo)
        BEGIN
            UPDATE ProdBillOfLading SET IsDeleted = 1 WHERE BillofladingNo = @p_BillofladingNo
        END
        INSERT INTO ProdBillOfLading (CreationTime, CreatorUserId, IsDeleted, BillofladingNo, ShipmentId, StatusCode)
                              VALUES (GETDATE(), @p_UserId, 0, @p_BillofladingNo, @p_ShipmentId, 'NEW');

        DECLARE @InvoiceNo NVARCHAR(20) = 'INVSH' + FORMAT(GETDATE(), 'yyMMddHHmmss'); 
        DECLARE @BillId INT = (SELECT Id FROM ProdBillOfLading WHERE BillofladingNo = @p_BillofladingNo);
        INSERT INTO ProdInvoice (CreationTime, CreatorUserId, IsDeleted, InvoiceNo, BillId, Status, Forwarder)
            SELECT GETDATE(), @p_UserId, 0, @InvoiceNo, @BillId, 'NEW', @p_Forwarder
              FROM ProdShipment 
             WHERE Id = @p_ShipmentId

        INSERT INTO ProdInvoiceDetails 
                    (CreationTime, CreatorUserId, IsDeleted, 
                     PartNo, ContainerNo, InvoiceNo, SupplierNo, UsageQty, PartName, CarfamilyCode, Currency)
            SELECT GETDATE(), @p_UserId, 0, pop.PartNo, pci.ContainerNo, @InvoiceNo, pci.SupplierNo, pci.UsageQty, pop.PartName, pop.CarfamilyCode, 'VND'
              FROM ProdContainerIntransit pci
         LEFT JOIN ProdOrderPart pop ON pci.PartListId = pop.Id
             WHERE pci.Id IN (SELECT Id FROM ProdContainerIntransit WHERE ShipmentId = @p_ShipmentId)
    END
END
------------------------------------------------Delete:
CREATE OR ALTER PROCEDURE INV_PROD_SHIPMENT_DELETE
    @p_ShipmentId INT,
    @p_UserId BIGINT
AS
BEGIN
    UPDATE ProdShipment SET IsDeleted = 1 WHERE Id = @p_ShipmentId;
    UPDATE ProdContainerIntransit
       SET LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId,
           ShipmentId = NULL,
           ShippingDate = NULL
     WHERE ShipmentId = @p_ShipmentId;
END
------------------------------------------------GetListShipmentNewOrPending:
CREATE OR ALTER PROCEDURE INV_PROD_SHIPMENT_GET_LIST_NEW_OR_PENDING
AS
BEGIN
    SELECT Id ShipmentId, ShipmentNo
      FROM ProdShipment 
     WHERE Status IN ('NEW', 'PENDING')
END
------------------------------------------------GetListShipmentNewOrPending:
CREATE OR ALTER PROCEDURE INV_PROD_SHIPMENT_GET_BY_ID
    @p_Id INT
AS
BEGIN
    SELECT a.Id ShipmentId, a.ShipmentNo, a.SupplierNo,
           a.Buyer, a.FromPort, a.ToPort, a.ShipmentDate,
           a.Etd, a.Eta, a.Ata, a.OceanVesselName, a.Atd, a.Status
      FROM ProdShipment a
     WHERE a.Id = @p_Id
END

------------------------------------------------listcont:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_CONTAINER_FOR_SHIPMENT
    @p_SupplierNo NVARCHAR(10)
AS
BEGIN
    SELECT a.Id, a.ContainerNo, pop.CarfamilyCode, a.UsageQty, pop.PartNo, pop.PartName
      FROM ProdContainerIntransit a
 LEFT JOIN ProdOrderPart pop ON a.PartListId = pop.Id
     WHERE a.SupplierNo = @p_SupplierNo
       AND a.Status = 'NEW'
		   AND a.ShipmentId IS NULL
       AND a.IsDeleted = 0
	ORDER BY a.ShippingDate DESC, a.PortDate DESC
END
------------------------------------------------listcont2:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_CONT_IN_SHIPMENT
    @p_ShipmentId INT
AS
BEGIN
    SELECT STRING_AGG(pci.Id, ',') ListCont 
      FROM ProdContainerIntransit pci 
     WHERE pci.ShipmentId = @p_ShipmentId
END
------------------------------------------------BillOfLading------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_PROD_BILL_OF_LADING_SEARCH
(
    @p_BillofladingNo NVARCHAR(20),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE
)
AS 
    SELECT DISTINCT b.Id, b.BillofladingNo, b.ShipmentId, b.BillDate, ps.ShipmentNo, b.StatusCode, ps.ShipmentDate
      FROM ProdBillOfLading b 
INNER JOIN ProdShipment ps
        ON b.ShipmentId = ps.Id
     WHERE (@p_BillofladingNo IS NULL OR b.BillofladingNo LIKE CONCAT('%', @p_BillofladingNo, '%'))
       AND (@p_BillDateFrom IS NULL OR b.BillDate >= @p_BillDateFrom)
       AND (@p_BillDateTo IS NULL OR b.BillDate <= @p_BillDateTo)
       AND b.IsDeleted = 0
  ORDER BY b.BillDate DESC
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_PROD_BILL_OF_LADING_EDIT
(
    @p_BillId INT, 
    @p_BillDate DATE,
    @p_StatusCode NVARCHAR(50),
    @p_UserId BIGINT
)
AS
BEGIN
    UPDATE ProdBillOfLading 
       SET LastModificationTime = GETDATE(), 
           LastModifierUserId = @p_UserId, 
           BillDate = @p_BillDate, 
           StatusCode = @p_StatusCode
     WHERE Id = @p_BillId;

    DECLARE @ShipmentId INT = (SELECT ShipmentId FROM ProdBillOfLading WHERE Id = @p_BillId);
    IF @p_StatusCode = 'PAID'
    BEGIN
        UPDATE ProdShipment
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               Status = 'ORDERED (ON SEA)'
         WHERE Id = @ShipmentId;

        UPDATE ProdContainerIntransit
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               Status = 'On SEA'
         WHERE Id IN (SELECT Id FROM ProdContainerIntransit WHERE ShipmentId = @ShipmentId);
    END
END
------------------------------------------------Delete:
CREATE OR ALTER PROCEDURE INV_PROD_BILL_OF_LADING_DELETE
(
    @p_BillId INT, 
    @p_UserId BIGINT
)
AS
BEGIN
    UPDATE ProdBillOfLading
       SET IsDeleted = 1,
           LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId
     WHERE Id = @p_BillId

    DECLARE @p_ShipmentId INT = (SELECT ShipmentId FROM ProdBillOfLading WHERE Id = @p_BillId);
    UPDATE ProdShipment
       SET LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId,
           Status = 'PENDING'
     WHERE Id = @p_ShipmentId
END
------------------------------------------------View:
CREATE OR ALTER PROCEDURE INV_PROD_BILL_OF_LADING_VIEW
    @p_BillId INT
AS
BEGIN
    DECLARE @ListContNo NVARCHAR(MAX);
    DECLARE @ListPartNo NVARCHAR(MAX);
    DECLARE @ListPartName NVARCHAR(MAX);
    DECLARE @ListCfc NVARCHAR(MAX);

     SELECT @ListContNo = STRING_AGG(pci.ContainerNo, ';'),
            @ListPartNo = STRING_AGG(pop.PartNo, ';'),
            @ListPartName = STRING_AGG(pop.PartName, ';'),
            @ListCfc = STRING_AGG(pop.CarfamilyCode, ';') 
       FROM ProdContainerIntransit pci
 INNER JOIN ProdOrderPart pop ON pci.PartListId = pop.Id
      WHERE pci.ShipmentId = (SELECT ShipmentId FROM ProdBillOfLading WHERE Id = @p_BillId )

    SELECT pbol.Id, pbol.BillofladingNo, pbol.BillDate, pbol.StatusCode, 
           ps.Forwarder, ps.SupplierNo, ps.FromPort, ps.ToPort, ps.OceanVesselName,
           @ListContNo ContainerNo, @ListPartNo PartNo, @ListPartName PartName, @ListCfc cfc
      FROM ProdBillOfLading pbol
INNER JOIN ProdShipment ps on pbol.ShipmentId = ps.Id
     WHERE pbol.Id = @p_BillId
END
------------------------------------------------Invoice------------------------------------------------
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_SEARCH 
(
    @p_InvoiceNo NVARCHAR(20),
    @p_InvoiceDateFrom DATE,
    @p_InvoiceDateTo DATE,
    @p_BillNo NVARCHAR(20),
    @p_ShipmentNo NVARCHAR(10),
    @p_ContainerNo NVARCHAR(20),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE,
    @p_SupplierNo NVARCHAR(10)
)
AS
BEGIN 
    SELECT DISTINCT a.Id, a.InvoiceNo, a.BillId, a.InvoiceDate, a.Status,  
           b.BillofladingNo AS BillNo, c.ShipmentNo, a.Forwarder,
           e.Description AS Status, b.BillDate
      FROM ProdInvoice a
 LEFT JOIN ProdBillOfLading b
        ON a.BillId = b.Id
 LEFT JOIN ProdShipment c
        ON c.Id = b.ShipmentId
 LEFT JOIN ProdContainerIntransit pci
        ON c.Id = pci.ShipmentId
 LEFT JOIN MasterInvoiceStatus e
        ON a.Status = e.Code
     WHERE (@p_InvoiceNo IS NULL OR a.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (@p_InvoiceDateFrom IS NULL OR a.InvoiceDate >= @p_InvoiceDateFrom)
       AND (@p_InvoiceDateTo IS NULL OR a.InvoiceDate <= @p_InvoiceDateTo)
       AND (@p_BillNo IS NULL OR b.BillofladingNo LIKE CONCAT('%', @p_BillNo, '%'))
       AND (@p_ShipmentNo IS NULL OR c.ShipmentNo LIKE CONCAT('%', @p_ShipmentNo, '%'))
       AND (@p_ContainerNo IS NULL OR pci.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_BillDateFrom IS NULL OR b.BillDate >= @p_BillDateFrom)
       AND (@p_BillDateTo IS NULL OR b.BillDate < DATEADD(DAY, 1, @p_BillDateTo))
	     AND (@p_SupplierNo IS NULL OR c.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND a.IsDeleted = 0
  ORDER BY a.InvoiceDate DESC, b.BillDate DESC, a.InvoiceNo, a.Id	
END
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_EDIT
(
    @p_InvoiceId INT, 
    @p_InvoiceDate DATE, 
    @p_Status NVARCHAR(50), 
    @p_UserId BIGINT
)
AS
BEGIN
    UPDATE ProdInvoice 
       SET LastModificationTime = GETDATE(), 
           LastModifierUserId = @p_UserId,
           InvoiceDate = @p_InvoiceDate,
           Status = @p_Status
     WHERE Id = @p_InvoiceId;
END
------------------------------------------------GetInvoiceCustomsDeclared:
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_CUSTOMS_DECLARED
AS
BEGIN
    SELECT pi.Id, pi.InvoiceNo, pi.InvoiceDate, pi.BillId, pi.Forwarder,
           pbol.BillofladingNo BillNo, pbol.BillDate
      FROM ProdInvoice pi
INNER JOIN ProdBillOfLading pbol ON pi.BillId = pbol.Id
     WHERE pi.Status = 'PRE CUSTOMS'
END
------------------------------------------------editdetails:
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_UPDATE_AMOUNT_INVOICE
(
    @p_InvoiceDetailsId INT,
    @p_Freight DECIMAL,
    @p_Insurance DECIMAL,
    @p_Tax DECIMAL, 
    @p_Vat DECIMAL, 
    @p_Thc DECIMAL, 
    @p_UserId BIGINT
)
AS
BEGIN
    UPDATE ProdInvoiceDetails
       SET LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId,
           Freight = @p_Freight,
           Insurance = @p_Insurance,
           Tax = @p_Tax,
           Vat = @p_Vat,
           Thc = @p_Thc,
           Cif = ISNULL(@p_Freight, 0) + ISNULL(@p_Insurance, 0)
     WHERE Id = @p_InvoiceDetailsId
END
------------------------------------------------InvoiceDetails------------------------------------------------
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_DETAILS_SEARCH
(
    @p_InvoiceId INT
)
AS 
BEGIN
    SELECT a.Id, a.PartNo, a.Insurance, a.ContainerNo, a.InvoiceNo, a.GrossWeight, a.Currency,  
           a.SupplierNo, a.Freight, a.Thc, a.Cif, a.Tax, a.TaxRate, a.Vat, a.VatRate, a.UsageQty, 
           a.PartName, a.CarfamilyCode
      FROM ProdInvoiceDetails a 
 LEFT JOIN ProdInvoice inv 
        ON inv.InvoiceNo = a.InvoiceNo 
     WHERE inv.Id = @p_InvoiceId
       AND a.IsDeleted = 0
  ORDER BY a.PartNo
END
------------------------------------------------ContainerList------------------------------------------------
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_LIST_SEARCH 
(
	  @p_ContainerNo NVARCHAR(20),
	  @p_SupplierNo NVARCHAR(10),
	  @p_BillOfLadingNo NVARCHAR(20),
	  @p_PortDateFrom DATE,
	  @p_PortDateTo DATE,
	  @p_ReceiveDateFrom DATE,
	  @p_ReceiveDateTo DATE,
	  @p_InvoiceNo NVARCHAR(20),
	  @p_ContainerStatus VARCHAR(1)
)
AS
BEGIN 
    SELECT DISTINCT a.ContainerNo, a.SupplierNo, pbol.BillofladingNo, a.ShippingDate, a.PortDate, pbol.BillDate, 
                    NULL ReceiveDate, pid.InvoiceNo, NULL Transport, NULL Remark, pid.Freight, 
                    pid.Insurance, pid.Cif, pid.Tax, (pid.Cif + pid.Tax) Amount, a.Status, NULL Warehouse
               FROM ProdContainerIntransit a
          LEFT JOIN ProdInvoiceDetails pid ON a.ContainerNo = pid.ContainerNo
          LEFT JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
          LEFT JOIN ProdBillOfLading pbol on pi.BillId = pbol.Id
              WHERE (@p_ContainerNo IS NULL OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%') 
                     OR a.ContainerNo IN (SELECT item FROM dbo.fnSplit(@p_ContainerNo, ';')) 
                    )
    		        AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
    		        AND (@p_BillOfLadingNo IS NULL OR pbol.BillofladingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
                AND (@p_PortDateFrom IS NULL OR a.PortDate >= @p_PortDateFrom)
                AND (@p_PortDateTo IS NULL OR a.PortDate <= @p_PortDateTo)
                AND (@p_InvoiceNo IS NULL OR pid.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
    		        AND (ISNULL(@p_ContainerStatus, '') = '' OR @p_ContainerStatus = '1') 
                AND a.IsDeleted = 0

    UNION ALL
    
    SELECT DISTINCT pcrw.ContainerNo, pcrw.SupplierNo, pbol.BillofladingNo, ps.ShipmentDate ShippingDate, ps.Atd PortDate, pbol.BillDate, 
                    pcrw.ReceiveDate, pid.InvoiceNo, pcrw.Transport, NULL Remark, pid.Freight, 
                    pid.Insurance, pid.Cif, pid.Tax, (pid.Cif + pid.Tax) Amount, 'DEVANNING' Status, pcrw.Warehouse 
               FROM ProdContainerRentalWHPlan pcrw
         INNER JOIN ProdBillOfLading pbol ON pcrw.BillId = pbol.Id
         INNER JOIN ProdShipment ps ON pbol.ShipmentId = ps.Id
         INNER JOIN ProdInvoiceDetails pid ON pid.InvoiceNo = (SELECT pi.InvoiceNo FROM ProdInvoice pi WHERE pcrw.InvoiceId = pi.Id) AND pcrw.ContainerNo = pid.ContainerNo
              WHERE (@p_ContainerNo IS NULL OR pcrw.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%') 
                     OR pcrw.ContainerNo IN (SELECT item FROM dbo.fnSplit(@p_ContainerNo, ';')) 
                    )
    		        AND (@p_SupplierNo IS NULL OR pcrw.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
    		        AND (@p_BillOfLadingNo IS NULL OR pbol.BillofladingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
                AND (@p_PortDateFrom IS NULL OR ps.Atd >= @p_PortDateFrom)
                AND (@p_PortDateTo IS NULL OR ps.Atd <= @p_PortDateTo)
                AND (@p_ReceiveDateFrom IS NULL OR pcrw.ReceiveDate >= @p_ReceiveDateFrom)
                AND (@p_ReceiveDateTo IS NULL OR pcrw.ReceiveDate <= @p_ReceiveDateTo)
                AND (@p_InvoiceNo IS NULL OR pid.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
                AND (ISNULL(@p_ContainerStatus, '') = '' OR @p_ContainerStatus = '2') 
                AND pcrw.IsDeleted = 0

    ORDER BY a.ShippingDate DESC, a.PortDate DESC, ReceiveDate DESC;
END
------------------------------------------------ContainerWarehouse------------------------------------------------
------------------------------------------------search:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_SEARCH
(
    @p_ContainerNo NVARCHAR(20),
    @p_InvoiceNo NVARCHAR(20),
    @p_BillOfLadingNo NVARCHAR(20),
    @p_SupplierNo NVARCHAR(10),
    @p_ReceiveDateFrom DATE,
    @p_ReceiveDateTo DATE,
    @p_Warehouse NVARCHAR(2)
)
AS
BEGIN 
    SELECT a.Id, a.ContainerNo, a.RequestDate, pi.InvoiceNo, pbol.BillofladingNo, a.ReceiveDate, 
           a.SupplierNo, a.Transport, a.Status, a.DeliveryDate, a.BillId, a.InvoiceId,
           (a.Warehouse + '/' + msl.AddressLanguageVn) Warehouse, 
           a.GoodsReceivedNoteNo, a.DevanningDate
      FROM ProdContainerRentalWHPlan a
INNER JOIN ProdInvoice pi ON a.InvoiceId = pi.Id
INNER JOIN ProdBillOfLading pbol ON a.BillId = pbol.Id
 LEFT JOIN MasterStorageLocation msl ON a.Warehouse = msl.StorageLocation
     WHERE (@p_ContainerNo IS NULL OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_InvoiceNo IS NULL OR pi.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (@p_BillOfLadingNo IS NULL OR pbol.BillofladingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
       AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_ReceiveDateFrom IS NULL OR a.ReceiveDate >= @p_ReceiveDateFrom)
       AND (@p_ReceiveDateTo IS NULL OR a.ReceiveDate <= @p_ReceiveDateTo)
       AND (@p_Warehouse IS NULL OR a.Warehouse = @p_Warehouse)
       AND a.IsDeleted = 0
  ORDER BY a.RequestDate DESC
END
------------------------------------------------edit:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_EDIT
@p_Id INT, 
@p_ContainerNo NVARCHAR(20),
@p_RequestDate DATE, 
@p_RequestTime TIME, 
@p_InvoiceNo NVARCHAR(20), 
@p_BillOfLadingNo NVARCHAR(20), 
@p_SupplierNo NVARCHAR(10), 
@p_SealNo NVARCHAR(20), 
@p_DevanningDate DATETIME2, 
@p_DevanningTime TIME, 
@p_ActualDevanningDate DATETIME2, 
@p_GateInPlanTime DATETIME2, 
@p_GateInActualDateTime DATETIME2, 
@p_Transport NVARCHAR(50), 
@p_Status NVARCHAR(10),
@p_UserId BIGINT
AS
BEGIN
    IF @p_Id IS NULL
    BEGIN
        INSERT INTO ProdContainerRentalWHPlan 
                    (CreationTime, CreatorUserId, IsDeleted, 
                     ContainerNo, RequestDate, RequestTime, InvoiceNo, BillofladingNo, 
                     SupplierNo, SealNo, DevanningDate, 
                     DevanningTime, ActualDevanningDate, GateInPlanTime, 
                     GateInActualDateTime, Transport, Status)
             VALUES (GETDATE(), @p_UserId, 0, 
                     @p_ContainerNo, @p_RequestDate, @p_RequestTime, @p_InvoiceNo, @p_BillOfLadingNo, 
                     @p_SupplierNo, @p_SealNo, @p_DevanningDate, 
                     @p_DevanningTime, @p_ActualDevanningDate, @p_GateInPlanTime, 
                     @p_GateInActualDateTime, @p_Transport, @p_Status);
    END
    ELSE
    BEGIN
        UPDATE ProdContainerRentalWHPlan
           SET ContainerNo = @p_ContainerNo,
               RequestDate = @p_RequestDate,
               RequestTime = @p_RequestTime,
               InvoiceNo = @p_InvoiceNo,
               BillofladingNo =@p_BillOfLadingNo,
               SupplierNo = @p_SupplierNo,
               SealNo = @p_SealNo,
               DevanningDate = @p_DevanningDate,
               DevanningTime = @p_DevanningTime,
               ActualDevanningDate = @p_ActualDevanningDate,
               GateInPlanTime = @p_GateInPlanTime,
               GateInActualDateTime = @p_GateInActualDateTime,
               Transport = @p_Transport,
               Status = @p_Status,
               LastModificationTime = GETDATE(),
               LastModifierUserId = @p_UserId
         WHERE Id = @p_Id
    END

END
------------------------------------------------GoodsReceivedNote:
CREATE OR ALTER PROCEDURE INV_PROD_EXPORT_GOODS_RECEIVED_NOTE
    @p_ContId NVARCHAR(MAX)
AS
BEGIN
    SELECT pid.PartNo, pid.PartName, pid.UsageQty, pi.Forwarder, pi.InvoiceNo,
           pid.ContainerNo, pop.BaseUnitOfMeasure, pop.AmountUnit StandardPrice
      FROM ProdInvoiceDetails pid
INNER JOIN ProdContainerIntransit pci ON pid.ContainerNo = pci.ContainerNo
INNER JOIN ProdOrderPart pop ON pci.PartListId = pop.Id
INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
     WHERE pci.Id IN (SELECT item FROM dbo.fnSplit(@p_ContId, ','))

    SELECT DISTINCT STRING_AGG(CONCAT(pid.InvoiceNo, ' - ', FORMAT(pi.InvoiceDate, 'dd/MM/yyyy')), '; ') InvoiceNo, 
           STRING_AGG(pi.Forwarder, '; ') Forwarder
      FROM ProdInvoiceDetails pid
INNER JOIN ProdContainerIntransit pci ON pid.ContainerNo = pci.ContainerNo
INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
     WHERE pci.Id IN (SELECT item FROM dbo.fnSplit(@p_ContId, ','))
END
------------------------------------------------AddGoodsReceivedNote:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_ADD_GOODS_RECEIVED_NOTE
    @p_GrnNo NVARCHAR(20),
    @p_ReceiveDate DATE,
    @p_Warehouse NVARCHAR(2),
    @p_ListContId NVARCHAR(MAX),
    @p_UserId BIGINT
AS
BEGIN
    DECLARE @Tranport NVARCHAR(MAX) = CASE WHEN @p_Warehouse LIKE 'A%' THEN N'Miền Bắc'
                                           WHEN @p_Warehouse LIKE 'B%' THEN N'Miền Trung'
                                      ELSE N'Miền Nam' END;
    PRINT 'INV_PROD_CONTAINER_WAREHOUSE_ADD_GOODS_RECEIVED_NOTE Start...';
    BEGIN TRY
    BEGIN TRANSACTION 
        DECLARE @p_value_cursor VARCHAR(255);
        DECLARE @p_id INT;
        DECLARE @p_qty INT;
        DECLARE @p_invoice_id INT;

        DECLARE cursor_value CURSOR FOR  
         SELECT value FROM STRING_SPLIT(@p_ListContId, ';');

           OPEN cursor_value 
FETCH NEXT FROM cursor_value INTO @p_ListContId

          WHILE @@FETCH_STATUS = 0  
          BEGIN  
            SET @p_id = SUBSTRING(@p_ListContId, 1, CHARINDEX('-', @p_ListContId) - 1);
            SET @p_qty = SUBSTRING(@p_ListContId, CHARINDEX('-', @p_ListContId) + 1, LEN(@p_ListContId) - CHARINDEX('-', @p_ListContId));
         
    INSERT INTO ProdContainerRentalWHPlan 
                (CreationTime, CreatorUserId, IsDeleted, 
                ContainerNo, SupplierNo, Transport, BillId, InvoiceId, ReceiveDate, Warehouse, GoodsReceivedNoteNo)
         SELECT GETDATE(), @p_UserId, 0, pci.ContainerNo, pci.SupplierNo, @Tranport, pi.BillId, pi.Id, @p_ReceiveDate, @p_Warehouse, @p_GrnNo 
           FROM ProdContainerIntransit pci 
     INNER JOIN ProdInvoiceDetails pid ON pci.ContainerNo = pid.ContainerNo
     INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
          WHERE pci.Id = @p_id

         UPDATE ProdContainerIntransit
            SET IsDeleted = 1
          WHERE Id = @p_id

    INSERT INTO ProdStockReceiving 
                (CreationTime, CreatorUserId, IsDeleted, 
                PartNo, PartName, PartListId, MaterialId, Qty, InvoiceDetailsId, 
                SupplierNo, Model, ActualQty, ContainerNo, Warehouse)
         SELECT GETDATE(), @p_UserId, 0, 
                pid.PartNo, pid.PartName, pci.PartListId, pop.MaterialId, pid.UsageQty, pid.Id, 
                pid.SupplierNo, pid.CarfamilyCode, @p_qty, pid.ContainerNo, @p_Warehouse 
           FROM ProdInvoiceDetails pid
     INNER JOIN ProdContainerIntransit pci ON pid.ContainerNo = pci.ContainerNo
     INNER JOIN ProdOrderPart pop ON pci.PartListId = pop.Id
          WHERE pci.Id = @p_id

FETCH NEXT FROM cursor_value INTO @p_ListContId 
            END  
          CLOSE cursor_value  
     DEALLOCATE cursor_value 
    PRINT 'INV_PROD_CONTAINER_WAREHOUSE_ADD_GOODS_RECEIVED_NOTE End...';
    COMMIT TRANSACTION;
    END TRY	
    BEGIN CATCH
    		DECLARE @ErrorSeverity INT;
    		DECLARE @ErrorState INT;
    		DECLARE @ErrorMessage NVARCHAR(4000);
      
    		SELECT @ErrorMessage = ERROR_MESSAGE(),
    			     @ErrorSeverity = ERROR_SEVERITY(), 
    			     @ErrorState = ERROR_STATE();
    
    		ROLLBACK TRANSACTION		
    		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
    		VALUES ('ProdContainerRentalWHPlan', 'ProdContainerRentalWHPlan Import', 'ERROR :' + @ErrorMessage + 
    															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
    															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
    															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
    															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
    															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
    	   
    	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END
------------------------------------------------delete:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_DELETE
    @p_Id INT
AS
BEGIN
    UPDATE ProdContainerRentalWHPlan
       SET IsDeleted = 1
     WHERE Id = @p_Id 
END
------------------------------------------------History:
CREATE OR ALTER PROCEDURE INV_PROD_HISTORY_RECEIVE
    @p_GRN NVARCHAR(20)
AS
BEGIN
    SELECT DISTINCT pcrw.GoodsReceivedNoteNo, pcrw.ReceiveDate, pcrw.Warehouse , 
           psr.PartNo, psr.Qty, psr.ActualQty, psr.PartName, pcrw.ContainerNo,
           pid.InvoiceNo, pcrw.SupplierNo, pi.Forwarder, pid.Freight, pid.Insurance, 
           pid.Cif, pid.Thc, pid.Tax, pid.Vat, pid.Currency, pid.CarfamilyCode,
           msl.AddressLanguageVn, mm.BaseUnitOfMeasure, mm.StandardPrice
      FROM ProdContainerRentalWHPlan pcrw
INNER JOIN ProdStockReceiving psr ON pcrw.ContainerNo = psr.ContainerNo
INNER JOIN ProdInvoiceDetails pid ON pcrw.ContainerNo = pid.ContainerNo
INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
 LEFT JOIN MasterStorageLocation msl ON msl.StorageLocation = pcrw.Warehouse
 LEFT JOIN MasterMaterial mm ON psr.MaterialId = mm.Id
     WHERE pcrw.GoodsReceivedNoteNo = @p_GRN
END

------------------------------------------------Import:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_MERGE
    @Guid VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY 
	  BEGIN TRANSACTION

	  -- FOR BUG CHECK-
		INSERT INTO ProcessLog(CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('ProdContainerRentalWHPlan', 'ProdContainerRentalWHPlan Import', 'START:', 'SYSTEM', GETDATE());

		UPDATE t1 
		   SET ErrorDescription = CONCAT(ISNULL(ErrorDescription, ''), N'Không tồn tại Container: ', t1.ContainerNo)
		  FROM ProdContainerRentalWHPlan_T t1
		 WHERE Guid = @Guid 
       AND NOT EXISTS (SELECT 1 FROM ProdContainerInvoice t2 WHERE t2.ContainerNo = t1.ContainerNo);

		IF NOT EXISTS (SELECT 1 FROM ProdContainerRentalWHPlan_T WHERE Guid = @Guid AND ErrorDescription != '')
		BEGIN			
	  	
		MERGE INTO ProdContainerRentalWHPlan AS P
		USING (
        	SELECT t1.ContainerNo, 
                 dbo.fn_DistinctList(STRING_AGG(t3.InvoiceNo, ','), ',') InvoiceNo,
                 dbo.fn_DistinctList(STRING_AGG(t4.BillofladingNo, ','), ',') BillofladingNo,
                 MAX(t2.SupplierNo) SupplierNo, t1.SealNo, 
                 t1.DevanningDate, t1.DevanningTime, t1.Transport, t1.CreatorUserId
        		FROM ProdContainerRentalWHPlan_T t1
      INNER JOIN ProdContainerInvoice t2 ON t1.ContainerNo = t2.ContainerNo
      INNER JOIN ProdInvoice t3 ON t3.Id = t2.InvoiceId
      INNER JOIN ProdBillOfLading t4 ON t3.BillId = t4.Id
        	 WHERE Guid = @Guid 
        GROUP BY t1.ContainerNo, t1.SealNo, t1.DevanningDate, t1.DevanningTime, t1.Transport, t1.CreatorUserId
		  ) AS DT ON (P.ContainerNo = DT.ContainerNo)				    
		WHEN MATCHED THEN
		    UPDATE SET 
					P.RequestDate = DT.DevanningDate,
					P.RequestTime = DT.DevanningTime,
					P.DevanningDate = DT.DevanningDate,
					P.DevanningTime = DT.DevanningTime,
					P.InvoiceNo = DT.InvoiceNo,
					P.BillofladingNo = DT.BillofladingNo,
					P.SealNo = DT.SealNo,
					P.Transport = DT.Transport,
					P.SupplierNo = DT.SupplierNo,
          P.LastModifierUserId = DT.CreatorUserId,
					P.LastModificationTime = GETDATE()
		WHEN NOT MATCHED THEN
		    INSERT (
                ContainerNo, InvoiceNo, BillofladingNo, SupplierNo, SealNo, 
                DevanningDate, DevanningTime, Status, Transport,
                RequestDate, RequestTime, 
                CreationTime, CreatorUserId, IsDeleted)				
		  	VALUES (
                ContainerNo, InvoiceNo, BillofladingNo, SupplierNo, SealNo,
                DT.DevanningDate, DevanningTime, 'R', Transport,
                DT.DevanningDate, DT.DevanningTime, 
                GETDATE(), CreatorUserId, 0);
		END

		-- FOR BUG CHECK-
		INSERT INTO ProcessLog (CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('ProdContainerRentalWHPlan', 'ProdContainerRentalWHPlan Import', 'END:', 'SYSTEM', GETDATE());

		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorMessage NVARCHAR(4000);
  
		SELECT @ErrorMessage = ERROR_MESSAGE(),
			     @ErrorSeverity = ERROR_SEVERITY(), 
			     @ErrorState = ERROR_STATE();

		ROLLBACK TRANSACTION		
		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
		VALUES ('ProdContainerRentalWHPlan', 'ProdContainerRentalWHPlan Import', 'ERROR :' + @ErrorMessage + 
															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
	   
	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	  END CATCH;
END
------------------------------------------------GetListErrorImport:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(MAX)
AS 
BEGIN
    SELECT DISTINCT Guid, ContainerNo, InvoiceNo, BillofladingNo, 
           SealNo, DevanningDate, DevanningTime, Transport, ErrorDescription
      FROM ProdContainerRentalWHPlan_T
     WHERE Guid = @Guid AND ISNULL(ErrorDescription, '') <> '' 
END
GO
------------------------------------------------ContainerIntransit------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_INTRANSIT_SEARCH
(
	  @p_ContainerNo NVARCHAR(20),
	  @p_ShippingDateFrom DATE,
    @p_ShippingDateTo DATE,
	  @p_PortDateFrom DATE,
    @p_PortDateTo DATE,
    @p_Status NVARCHAR(50)
)
AS
BEGIN
    SELECT a.Id, a.ContainerNo, a.SupplierNo, a.ShippingDate, a.PortDate, pop.CarfamilyCode,
           a.ShipmentId, a.Status, a.UsageQty, a.PartListId, ps.ShipmentNo, pop.PartNo
      FROM ProdContainerIntransit a
 LEFT JOIN ProdShipment ps ON a.ShipmentId = ps.Id
 LEFT JOIN ProdOrderPart pop ON a.PartListId = pop.Id
     WHERE (ISNULL(@p_ContainerNo, '') = '' OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
    	 AND (ISNULL(@p_ShippingDateFrom, '') = '' OR a.ShippingDate >= @p_ShippingDateFrom)
       AND (ISNULL(@p_ShippingDateTo, '') = '' OR a.ShippingDate <= @p_ShippingDateTo)
    	 AND (ISNULL(@p_PortDateFrom, '') = '' OR a.PortDate >= @p_PortDateFrom)
       AND (ISNULL(@p_PortDateTo, '') = '' OR a.PortDate <= @p_PortDateTo)
       AND (ISNULL(@p_Status, '') = '' OR a.Status = @p_Status)
		   AND a.IsDeleted = 0
	ORDER BY a.ShippingDate DESC, a.PortDate DESC
END
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_INTRANSIT_EDIT
(
    @p_ContId INT,
    @p_ContainerNo NVARCHAR(20),
    @p_SupplierNo NVARCHAR(10),
    @p_PartListId INT,
    @p_UsageQty INT,
    @p_UserId BIGINT
)
AS
BEGIN
    IF @p_ContId IS NULL
    BEGIN
        INSERT INTO ProdContainerIntransit 
                    (CreationTime, CreatorUserId, IsDeleted, 
                    ContainerNo, SupplierNo, Status, UsageQty, PartListId)
             VALUES (GETDATE(), @p_UserId, 0, 
                    @p_ContainerNo, @p_SupplierNo, 'NEW', @p_UsageQty, @p_PartListId);
    END
    ELSE
    BEGIN
        UPDATE ProdContainerIntransit 
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId, 
               UsageQty = @p_UsageQty,
               PartListId = @p_PartListId,
               SupplierNo = @p_SupplierNo
         WHERE Id = @p_ContId;
    END
END
------------------------------------------------import:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_INTRANSIT_MERGE
    @Guid VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY 
	  BEGIN TRANSACTION

	  -- FOR BUG CHECK-
		INSERT INTO ProcessLog(CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('INV_PROD_CONTAINER_INTRANSIT_MERGE', 'INV_PROD_CONTAINER_INTRANSIT_MERGE Import', 'START:', 'SYSTEM', GETDATE());

		UPDATE t1 
		   SET ErrorDescription = CONCAT(ISNULL(ErrorDescription, ''), N' Đã tồn tại ContainerNo: ', t1.ContainerNo, N' trong kho! ')
		  FROM ProdContainerIntransit_T t1
		 WHERE Guid = @Guid 
       AND EXISTS (SELECT 1 FROM ProdContainerIntransit t2 WHERE t2.ContainerNo = t1.ContainerNo AND t2.Status = 'NEW');

    UPDATE t1 
		   SET ErrorDescription = CONCAT(ISNULL(ErrorDescription, ''), N' Không tồn tại PartNo - Cfc - SupplierNo: ', t1.PartNo, '-',t1.CarfamilyCode, '-', t1.SupplierNo)
		  FROM ProdContainerIntransit_T t1
		 WHERE Guid = @Guid 
       AND NOT EXISTS (SELECT 1 FROM MasterPartList t2 WHERE t2.PartNo = t1.PartNo AND t2.SupplierNo = t1.SupplierNo AND t2.CarfamilyCode = t1.CarfamilyCode);

		IF NOT EXISTS (SELECT 1 FROM ProdContainerIntransit_T WHERE Guid = @Guid AND ErrorDescription != '')
		BEGIN			
	  	
		MERGE INTO ProdContainerIntransit AS P
		USING (
        	SELECT t1.ContainerNo, t2.Id PartListId, t1.SupplierNo, t1.CreatorUserId, t1.UsageQty, t1.CarfamilyCode
        		FROM ProdContainerIntransit_T t1
      INNER JOIN MasterPartList t2 ON t1.PartNo = t2.PartNo AND t1.SupplierNo = t2.SupplierNo AND t2.CarfamilyCode = t1.CarfamilyCode
        	 WHERE Guid = @Guid 
		  ) AS DT ON (P.ContainerNo = DT.ContainerNo)				    
		WHEN MATCHED THEN
		    UPDATE SET 
					P.PartListId = DT.PartListId,
					P.SupplierNo = DT.SupplierNo,
          P.UsageQty = DT.UsageQty,
          P.LastModifierUserId = DT.CreatorUserId,
					P.LastModificationTime = GETDATE()
		WHEN NOT MATCHED THEN
		    INSERT (ContainerNo, SupplierNo, PartListId, UsageQty, Status, 
                CreationTime, CreatorUserId, IsDeleted)				
		  	VALUES (DT.ContainerNo, DT.SupplierNo, DT.PartListId, DT.UsageQty, 'NEW', 
                GETDATE(), CreatorUserId, 0);
		END

		-- FOR BUG CHECK-
		INSERT INTO ProcessLog (CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('INV_PROD_CONTAINER_INTRANSIT_MERGE', 'INV_PROD_CONTAINER_INTRANSIT_MERGE Import', 'END:', 'SYSTEM', GETDATE());

		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorMessage NVARCHAR(4000);
  
		SELECT @ErrorMessage = ERROR_MESSAGE(),
			     @ErrorSeverity = ERROR_SEVERITY(), 
			     @ErrorState = ERROR_STATE();

		ROLLBACK TRANSACTION		
		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
		VALUES ('INV_PROD_CONTAINER_INTRANSIT_MERGE', 'INV_PROD_CONTAINER_INTRANSIT_MERGE Import', 'ERROR :' + @ErrorMessage + 
															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
	   
	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	  END CATCH;
END
------------------------------------------------ListErrorImport:
CREATE OR ALTER PROCEDURE INV_PROD_CONTAINER_INTRANSIT_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(MAX)
AS 
BEGIN
    SELECT DISTINCT Guid, ContainerNo, SupplierNo, PartNo, UsageQty, CarfamilyCode, ErrorDescription
      FROM ProdContainerIntransit_T
     WHERE Guid = @Guid AND ISNULL(ErrorDescription, '') <> '' 
END
------------------------------------------------GetListToWarehouse:
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_CONTAINER_TO_WAREHOUSE
AS
BEGIN
        SELECT pci.Id, pci.ContainerNo, pci.SupplierNo, pci.UsageQty,
               pi.Forwarder, pid.Insurance, pid.Freight, pid.Thc, 
               pid.Cif, pid.Tax, pid.Vat, pid.Currency, pi.InvoiceNo,
               pi.InvoiceDate, pid.PartNo, pid.PartName, pid.CarfamilyCode
          FROM ProdContainerIntransit pci 
    INNER JOIN ProdInvoiceDetails pid ON pci.ContainerNo = pid.ContainerNo
    INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
         WHERE pci.Status = 'PORT/ARRIVED'
           AND pci.IsDeleted = 0
END
------------------------------------------------StockReceving------------------------------------------------
------------------------------------------------Search
CREATE OR ALTER PROCEDURE INV_PROD_STOCK_RECEIVING_SEARCH 
(
	  @p_PartNo NVARCHAR(12),
	  @p_RequestDateFrom DATE,
	  @p_RequestDateTo DATE,
    @p_SupplierNo NVARCHAR(15),
    @p_Model NVARCHAR(4),
    @p_Warehouse NVARCHAR(2),
    @p_StockStatus NVARCHAR(1)
)
AS
BEGIN
	  SELECT r.Id, r.PartNo, r.PartName, r.PartListId, r.MaterialId, 
           ISNULL(r.Qty, 0) Qty, r.InvoiceDetailsId, r.RequestDate,
		       r.SupplierNo, r.ContainerNo, d.InvoiceNo, r.Model, 
           r.ActualQty, r.OrderQty, r.InvoiceNoOut, r.RequestStatus,
           r.DeliveryDate, (r.Warehouse + '/' + msl.AddressLanguageVn) Warehouse,
           ISNULL(r.OrderedQty, 0) OrderedQty, (r.ActualQty - ISNULL(r.OrderedQty, 0)) RemainQty,
           mm.StandardPrice, ISNULL(mm.MovingPrice, 0) MovingPrice, 
           (mm.StandardPrice * r.OrderQty + ISNULL(mm.MovingPrice, 0)) AmountOrder
      FROM ProdStockReceiving r
INNER JOIN ProdInvoiceDetails d ON d.Id = r.InvoiceDetailsId
 LEFT JOIN MasterStorageLocation msl ON r.Warehouse = msl.StorageLocation
 LEFT JOIN MasterMaterial mm ON r.MaterialId = mm.Id
	   WHERE (ISNULL(@p_PartNo, '') = '' OR r.PartNo LIKE CONCAT('%', @p_PartNo, '%'))
		   AND (ISNULL(@p_RequestDateFrom, '')= '' OR  r.RequestDate >= @p_RequestDateFrom)
       AND (ISNULL(@p_RequestDateTo, '')= '' OR r.RequestDate <= @p_RequestDateTo)
       AND (ISNULL(@p_SupplierNo, '') = '' OR r.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND r.Warehouse = @p_Warehouse
       AND (ISNULL(@p_Model, '') = '' OR r.Model LIKE CONCAT('%', @p_Model, '%'))
       AND (ISNULL(@p_StockStatus, '') = '' OR (@p_StockStatus = '1' AND (r.DeliveryDate IS NULL OR (0 < (r.ActualQty - ISNULL(r.OrderedQty, 0)))))  
                                            OR (@p_StockStatus = '2' AND r.RequestStatus = 'NEW')
                                            OR (@p_StockStatus = '3' AND r.DeliveryDate IS NOT NULL)
           )
       AND r.IsDeleted = 0
  ORDER BY r.Model, r.PartNo, r.RequestDate DESC, r.DeliveryDate DESC
END
------------------------------------------------GetListForOrder
CREATE OR ALTER PROCEDURE INV_PROD_GET_LIST_ORDER_BY_ID
    @p_ListPartId NVARCHAR(MAX)
AS
BEGIN
    SELECT psr.Id, psr.PartNo, psr.PartName, psr.SupplierNo, psr.Model Cfc, psr.Warehouse,
           (psr.ActualQty - ISNULL(psr.OrderedQty, 0)) Qty, mm.StandardPrice, ISNULL(mm.MovingPrice, 0) MovingPrice,
           ((psr.ActualQty - ISNULL(psr.OrderedQty, 0)) * mm.StandardPrice + ISNULL(mm.MovingPrice, 0)) Amount
      FROM ProdStockReceiving psr
 LEFT JOIN MasterMaterial mm ON psr.MaterialId = mm.Id
     WHERE psr.Id IN (SELECT item FROM dbo.fnSplit(@p_ListPartId, ','))
END
------------------------------------------------PurchaseOrder
CREATE OR ALTER PROCEDURE INV_PROD_CONFIRM_PURCHASE_ORDER
    @p_InvoiceOut NVARCHAR(20),
    @p_RequestDate DATE,
    @p_ListOrder NVARCHAR(MAX),
    @p_UserId BIGINT
AS
BEGIN
    PRINT 'INV_PROD_CONFIRM_PURCHASE_ORDER Start...';
    BEGIN TRY
    BEGIN TRANSACTION 
        DECLARE @p_value_cursor VARCHAR(255);
        DECLARE @p_id INT;
        DECLARE @p_qty INT;

        DECLARE cursor_value CURSOR FOR  
         SELECT value FROM STRING_SPLIT(@p_ListOrder, ';');

           OPEN cursor_value 
FETCH NEXT FROM cursor_value INTO @p_ListOrder

          WHILE @@FETCH_STATUS = 0  
          BEGIN  
            SET @p_id = SUBSTRING(@p_ListOrder, 1, CHARINDEX('-', @p_ListOrder) - 1);
            SET @p_qty = SUBSTRING(@p_ListOrder, CHARINDEX('-', @p_ListOrder) + 1, LEN(@p_ListOrder) - CHARINDEX('-', @p_ListOrder));
         
          DECLARE @InvoiceOutNo NVARCHAR(20) = (SELECT InvoiceNoOut FROM ProdStockReceiving WHERE Id = @p_id);
         IF @InvoiceOutNo IS NULL
         BEGIN
             UPDATE ProdStockReceiving
                SET LastModificationTime = GETDATE(), 
                    LastModifierUserId = @p_UserId, 
                    RequestDate = @p_RequestDate,
                    InvoiceNoOut = @p_InvoiceOut,
                    OrderQty = @p_qty,
                    RequestStatus = 'NEW'
              WHERE Id = @p_id
    
             INSERT INTO ProdInvoiceStockOut 
                    (CreationTime, CreatorUserId, IsDeleted, 
                    InvoiceNoOut, Status, ListPartNo, ListPartName, ListCfc, ListStockId, TotalOrderQty, TotalAmount)
             SELECT GETDATE(), @p_UserId, 0, @p_InvoiceOut, 'NEW', a.PartNo, a.PartName, a.Model, a.Id, @p_qty, (@p_qty * mm.StandardPrice + ISNULL(mm.MovingPrice, 0))
               FROM ProdStockReceiving a
          LEFT JOIN MasterMaterial mm ON a.MaterialId = mm.Id
              WHERE a.Id = @p_id
         END
         ELSE
         BEGIN
             DECLARE @Count INT = (SELECT COUNT(*) FROM ProdInvoiceStockOut WHERE InvoiceNoOut LIKE CONCAT(@InvoiceOutNo, '%'));
             UPDATE ProdStockReceiving
                SET LastModificationTime = GETDATE(), 
                    LastModifierUserId = @p_UserId, 
                    RequestDate = @p_RequestDate,
                    OrderQty = @p_qty,
                    RequestStatus = 'NEW'
              WHERE Id = @p_id
    
             INSERT INTO ProdInvoiceStockOut 
                    (CreationTime, CreatorUserId, IsDeleted, 
                    InvoiceNoOut, Status, ListPartNo, ListPartName, ListCfc, ListStockId, TotalOrderQty, TotalAmount)
             SELECT GETDATE(), @p_UserId, 0, (@InvoiceOutNo + '/' + CONVERT(NVARCHAR, @Count)), 'NEW', a.PartNo, a.PartName, a.Model, a.Id, @p_qty, (@p_qty * mm.StandardPrice + ISNULL(mm.MovingPrice, 0))
               FROM ProdStockReceiving a
          LEFT JOIN MasterMaterial mm ON a.MaterialId = mm.Id
              WHERE a.Id = @p_id
         END

FETCH NEXT FROM cursor_value INTO @p_ListOrder 
            END  
          CLOSE cursor_value  
     DEALLOCATE cursor_value 
    PRINT 'INV_PROD_CONFIRM_PURCHASE_ORDER End...';
    COMMIT TRANSACTION;
    END TRY	
    BEGIN CATCH
    		DECLARE @ErrorSeverity INT;
    		DECLARE @ErrorState INT;
    		DECLARE @ErrorMessage NVARCHAR(4000);
      
    		SELECT @ErrorMessage = ERROR_MESSAGE(),
    			     @ErrorSeverity = ERROR_SEVERITY(), 
    			     @ErrorState = ERROR_STATE();
    
    		ROLLBACK TRANSACTION		
    		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
    		VALUES ('INV_PROD_CONFIRM_PURCHASE_ORDER', 'INV_PROD_CONFIRM_PURCHASE_ORDER', 'ERROR :' + @ErrorMessage + 
    															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
    															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
    															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
    															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
    															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
    	   
    	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END
------------------------------------------------UpdateOrderQty:
CREATE OR ALTER PROCEDURE INV_PROD_UPDATE_ORDER_QTY_STOCK
    @p_StockId INT, 
    @p_OrderQty INT, 
    @p_UserId BIGINT
AS
BEGIN
    UPDATE ProdStockReceiving 
       SET LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId,
           OrderQty = @p_OrderQty
     WHERE Id = @p_StockId;
END
------------------------------------------------GetListForDelivery
CREATE OR ALTER PROCEDURE INV_PROD_GET_STOCK_FOR_DELIVERY_BY_WAREHOUSE
    @p_Warehouse NVARCHAR(2)
AS
BEGIN
    SELECT r.Id, r.PartNo, r.PartName, mm.BaseUnitOfMeasure, r.Model, 
           r.OrderQty, r.InvoiceNoOut, r.Warehouse, r.RequestDate,
           (r.ActualQty - ISNULL(r.OrderedQty, 0)) RemainQty,
           mm.StandardPrice, ISNULL(mm.MovingPrice, 0) MovingPrice, 
           (mm.StandardPrice * r.OrderQty + ISNULL(mm.MovingPrice, 0)) AmountOrder,
           msl.AddressLanguageVn Location
      FROM ProdStockReceiving r
 LEFT JOIN MasterStorageLocation msl ON r.Warehouse = msl.StorageLocation
 LEFT JOIN MasterMaterial mm ON r.MaterialId = mm.Id
	   WHERE r.RequestDate IS NOT NULL 
       AND r.RequestStatus = 'NEW'
       AND r.IsDeleted = 0
       AND r.Warehouse = @p_Warehouse
       AND r.RequestDate <= GETDATE()
END
------------------------------------------------ExportListForDelivery
CREATE PROCEDURE INV_PROD_EXPORT_GOODS_DELIVERY_NOTE
    @p_StockId NVARCHAR(MAX)
AS
BEGIN
    SELECT psr.PartNo, psr.PartName, mm.BaseUnitOfMeasure, mm.StandardPrice, 
           ISNULL(mm.MovingPrice, 0) MovingPrice, psr.OrderQty
      FROM ProdStockReceiving psr
INNER JOIN MasterMaterial mm ON psr.MaterialId = mm.Id
     WHERE psr.Id IN (SELECT item FROM dbo.fnSplit(@p_StockId, ','))

    SELECT DISTINCT CONCAT(psr.InvoiceNoOut, ' - ', FORMAT(psr.RequestDate, 'dd/MM/yyyy')) InvoiceNoOut
      FROM ProdStockReceiving psr
     WHERE psr.Id IN (SELECT item FROM dbo.fnSplit(@p_StockId, ','))
END
------------------------------------------------Final
CREATE OR ALTER PROCEDURE INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE
    @p_GdnNo NVARCHAR(20),
    @p_InvoiceOutDate DATE,
    @p_ListStockId NVARCHAR(MAX),
    @p_UserId BIGINT
AS
BEGIN
    PRINT 'INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE Start...';
    BEGIN TRY
    BEGIN TRANSACTION 
        DECLARE @p_value_cursor VARCHAR(255);
        DECLARE @p_id INT;
        DECLARE @p_qty INT;

        DECLARE cursor_value CURSOR FOR  
         SELECT value FROM STRING_SPLIT(@p_ListStockId, ';');

           OPEN cursor_value 
FETCH NEXT FROM cursor_value INTO @p_ListStockId

          WHILE @@FETCH_STATUS = 0  
          BEGIN  
            SET @p_id = SUBSTRING(@p_ListStockId, 1, CHARINDEX('-', @p_ListStockId) - 1);
            SET @p_qty = SUBSTRING(@p_ListStockId, CHARINDEX('-', @p_ListStockId) + 1, LEN(@p_ListStockId) - CHARINDEX('-', @p_ListStockId));
         
         UPDATE ProdStockReceiving
            SET LastModificationTime = GETDATE(),
                LastModifierUserId = @p_UserId,
                OrderedQty = ISNULL(OrderedQty, 0) + @p_qty,
                OrderQty = 0,
                RequestStatus = 'DELIVERED',
                DeliveryDate = @p_InvoiceOutDate
          WHERE Id = @p_id

        DECLARE @InvoiceStockOut NVARCHAR(20) = (SELECT psr.InvoiceNoOut FROM ProdStockReceiving psr WHERE psr.Id = @p_id);
        DECLARE @Amount DECIMAL;
        DECLARE @PartNo NVARCHAR(12);
        DECLARE @Cfc NVARCHAR(4);
         SELECT @Amount = (@p_qty * mm.StandardPrice + ISNULL(mm.MovingPrice, 0)), 
                @PartNo = psr.PartNo, @Cfc = psr.Model
           FROM ProdStockReceiving psr 
      LEFT JOIN MasterMaterial mm ON psr.MaterialId = mm.Id 
          WHERE psr.Id = @p_id

         UPDATE ProdInvoiceStockOut
            SET LastModificationTime = GETDATE(),
                LastModifierUserId = @p_UserId,
                InvoiceDate = @p_InvoiceOutDate,
                STATUS = 'NOT PAID (REQUESTED)',
                GoodsDeliveryNoteNo = @p_GdnNo,
                TotalOrderQty = @p_qty,
                TotalAmount = @Amount
          WHERE InvoiceNoOut LIKE CONCAT(@InvoiceStockOut, '%') AND ListPartNo = @PartNo AND ListCfc = @Cfc AND InvoiceDate IS NULL

FETCH NEXT FROM cursor_value INTO @p_ListStockId 
            END  
          CLOSE cursor_value  
     DEALLOCATE cursor_value 
    PRINT 'INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE End...';
    COMMIT TRANSACTION;
    END TRY	
    BEGIN CATCH
    		DECLARE @ErrorSeverity INT;
    		DECLARE @ErrorState INT;
    		DECLARE @ErrorMessage NVARCHAR(4000);
      
    		SELECT @ErrorMessage = ERROR_MESSAGE(),
    			     @ErrorSeverity = ERROR_SEVERITY(), 
    			     @ErrorState = ERROR_STATE();
    
    		ROLLBACK TRANSACTION		
    		INSERT INTO ProCess_Log ([CATEGORY], [PROCESS_NAME], [ERROR_MESSAGE], [CREATED_DATE])
    		VALUES ('INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE', 'INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE', 'ERROR :' + @ErrorMessage + 
    															'//ERRORSTATE :' +  CAST(@ErrorState AS VARCHAR) + 
    															'//ERRORPROCEDURE :' + ERROR_PROCEDURE() + 
    															'//ERRORSEVERITY :' + CAST(@ErrorSeverity AS VARCHAR) + 
    															'//ERRORNUMBER :' + CAST(ERROR_NUMBER() AS VARCHAR) + 
    															'//ERRORLINE :' + CAST(ERROR_LINE() AS VARCHAR), GETDATE());
    	   
    	  RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END
------------------------------------------------CustomsDeclare------------------------------------------------
------------------------------------------------Search:
CREATE OR ALTER PROCEDURE INV_PROD_CUSTOMS_DECLARE_SEARCH 
(
    @p_CustomsDeclareNo NVARCHAR(20),
    @p_DeclareDateFrom DATE,
    @p_DeclareDateTo DATE,
    @p_BillOfLadingNo NVARCHAR(20),
    @p_InvoiceNo NVARCHAR(20),
    @p_Status NVARCHAR(50)
)
AS
BEGIN
    SELECT pcd.Id, pcd.CustomsDeclareNo, pcd.DeclareDate, pcd.Tax, pcd.Vat, pcd.Forwarder, pcd.BillId,
           pbol.BillofladingNo, pbol.BillDate, (pcd.Tax + pcd.Vat) SumCustomsDeclare, mcs.Description Status,
           pcd.InvoiceId, pi.InvoiceNo, pi.InvoiceDate
      FROM ProdCustomsDeclare pcd 
INNER JOIN ProdBillOfLading pbol ON pcd.BillId = pbol.Id
INNER JOIN ProdInvoice pi ON pcd.InvoiceId = pi.Id
 LEFT JOIN MasterCustomsStatus mcs ON pcd.Status = mcs.Code
     WHERE (ISNULL(@p_CustomsDeclareNo, '') = '' OR pcd.CustomsDeclareNo LIKE CONCAT('%', @p_CustomsDeclareNo, '%'))
		   AND (ISNULL(@p_DeclareDateFrom, '')= '' OR  pcd.DeclareDate >= @p_DeclareDateFrom)
       AND (ISNULL(@p_DeclareDateTo, '')= '' OR  pcd.DeclareDate <= @p_DeclareDateTo)
       AND (ISNULL(@p_BillOfLadingNo, '') = '' OR pbol.BillofladingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
       AND (ISNULL(@p_InvoiceNo, '') = '' OR pi.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (ISNULL(@p_Status, '') = '' OR pcd.Status = @p_Status)
       AND pcd.IsDeleted = 0
  ORDER BY pcd.DeclareDate DESC
END
------------------------------------------------UpdateStatus:
CREATE OR ALTER PROCEDURE INV_PROD_CUSTOMS_DECLARE_EDIT
(
    @p_CustomsDeclareId INT,
    @p_CustomsDeclareNo NVARCHAR(20),
    @p_DeclareDate DATE,
    @p_Tax DECIMAL,
    @p_Vat DECIMAL,
	  @p_Status NVARCHAR(4),
    @p_Forwarder NVARCHAR(10),
    @p_BillId INT,
    @p_InvoiceId INT,
    @p_UserId BIGINT
)
AS
BEGIN
    IF @p_CustomsDeclareId IS NULL
        INSERT INTO ProdCustomsDeclare (CreationTime, CreatorUserId, IsDeleted, 
                                        CustomsDeclareNo, DeclareDate, Tax, Vat, 
                                        Status, Forwarder, BillId, InvoiceId)
                                VALUES (GETDATE(), @p_UserId, 0, 
                                        @p_CustomsDeclareNo, @p_DeclareDate, @p_Tax, @p_Vat, 
                                        @p_Status, @p_Forwarder, @p_BillId, @p_InvoiceId);
    ELSE
    BEGIN
        UPDATE ProdCustomsDeclare 
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId, 
               DeclareDate = @p_DeclareDate, 
               Tax = @p_Tax, 
               Vat = @p_Vat, 
               Status = @p_Status
         WHERE Id = @p_CustomsDeclareId;
    END

    IF @p_Status = 'CuS3'
    BEGIN
        UPDATE ProdInvoice
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               Status = 'CUSTOMS DECLARED'
         WHERE Id = @p_InvoiceId

        UPDATE ProdShipment
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               Status = 'ORDERED (ON PORT)',
               Atd = @p_DeclareDate
         WHERE Id = (SELECT ShipmentId FROM ProdBillOfLading WHERE Id = @p_BillId);

        UPDATE ProdContainerIntransit
           SET LastModificationTime = GETDATE(), 
               LastModifierUserId = @p_UserId,
               PortDate = @p_DeclareDate,
               Status = 'PORT/ARRIVED'
         WHERE Id IN (SELECT ps.Id FROM ProdContainerIntransit ps 
                      WHERE ps.ShipmentId = (SELECT ShipmentId FROM ProdBillOfLading WHERE Id = @p_BillId))
    END
END
------------------------------------------------InvoiceStockOut------------------------------------------------
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_STOCK_OUT_SEARCH
(
    @p_InvoiceNoOut NVARCHAR(20),
    @p_InvoiceDateFrom DATE,
    @p_InvoiceDateTo DATE,
    @p_Status NVARCHAR(50),
    @p_Warehouse NVARCHAR(2)
)
AS
BEGIN
    SELECT piso.Id, piso.ListStockId, piso.InvoiceNoOut, piso.InvoiceDate, 
           piso.Status, piso.ListPartNo, piso.ListPartName, piso.ListCfc, 
           piso.ListStockId, piso.TotalOrderQty, piso.TotalAmount, 
           (psr.Warehouse + '/' + msl.AddressLanguageVn) Warehouse
      FROM ProdInvoiceStockOut piso
INNER JOIN ProdStockReceiving psr ON psr.Id = piso.ListStockId
 LEFT JOIN MasterStorageLocation msl ON psr.Warehouse = msl.StorageLocation
     WHERE (@p_InvoiceNoOut IS NULL OR piso.InvoiceNoOut LIKE CONCAT('%', @p_InvoiceNoOut, '%'))
       AND (@p_InvoiceDateFrom IS NULL OR piso.InvoiceDate >= @p_InvoiceDateFrom)
       AND (@p_InvoiceDateTo IS NULL OR piso.InvoiceDate <= @p_InvoiceDateTo)
       AND (@p_Status IS NULL OR piso.Status LIKE CONCAT('%', @p_Status, '%'))
       AND (@p_Warehouse IS NULL OR psr.Warehouse = @p_Warehouse)
       AND piso.IsDeleted = 0
  ORDER BY psr.Warehouse, piso.InvoiceNoOut
END
------------------------------------------------updateStatus:
CREATE OR ALTER PROCEDURE INV_PROD_INVOICE_STOCK_UPDATE_STATUS
    @p_InvoiceStockId INT,
    @p_NewStatus NVARCHAR(50),
    @p_UserId BIGINT
AS
BEGIN
    UPDATE ProdInvoiceStockOut
       SET LastModificationTime = GETDATE(),
           LastModifierUserId = @p_UserId,
           Status = @p_NewStatus
     WHERE Id = @p_InvoiceStockId;
END
------------------------------------------------ORDER_PART------------------------------------------------
------------------------------------------------Search
CREATE OR ALTER PROCEDURE INV_PROD_ORDER_PART_SEARCH
(
    @p_PartNo NVARCHAR(15),
    @p_SupplierNo NVARCHAR(10),
    @p_Cfc NVARCHAR(4),
    @p_ContainerNo NVARCHAR(20),
    @p_ShimentNo NVARCHAR(20),
    @p_Status NVARCHAR(50),
    @p_OrderDateFrom DATE,
    @p_OrderDateTo DATE
)
AS
BEGIN
    SELECT pop.Id, pop.PartNo, pop.PartName, pop.SupplierNo, pop.CarfamilyCode, 
           pop.SupplierId, pop.MaterialId, pop.Status, pop.Remark, pop.ShipmentId, 
           pop.ContainerNo, pop.Qty, pop.AmountUnit, pop.TotalAmount, ps.ShipmentNo,
           pop.OrderDate, pop.BaseUnitOfMeasure
      FROM ProdOrderPart pop
 LEFT JOIN ProdShipment ps on pop.ShipmentId = ps.Id
     WHERE (@p_PartNo IS NULL OR pop.PartNo LIKE CONCAT('%', @p_PartNo, '%'))
       AND (@p_SupplierNo IS NULL OR pop.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_Cfc IS NULL OR pop.CarfamilyCode LIKE CONCAT('%', @p_Cfc, '%'))
       AND (@p_ContainerNo IS NULL OR pop.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_ShimentNo IS NULL OR ps.ShipmentNo LIKE CONCAT('%', @p_ShimentNo, '%'))
       AND (@p_Status IS NULL OR pop.Status = @p_Status)
       AND (@p_OrderDateFrom IS NULL OR pop.OrderDate >= @p_OrderDateFrom)
       AND (@p_OrderDateTo IS NULL OR pop.OrderDate <= @p_OrderDateTo)
       AND pop.IsDeleted = 0
END
------------------------------------------------Edit:
CREATE OR ALTER PROCEDURE INV_PROD_ORDER_PART_EDIT
(
    @p_OrderId INT,
    @p_PartNo NVARCHAR(15),
    @p_PartName NVARCHAR(MAX),
    @p_SupplierNo NVARCHAR(10),
    @p_Cfc NVARCHAR(4),
    @p_Status NVARCHAR(50),
    @p_Remark NVARCHAR(MAX),
    @p_Qty INT,
    @p_AmountUnit DECIMAL,
    @p_TotalAmount DECIMAL,
    @p_OrderDate DATE,
    @p_BOM NVARCHAR(3),
    @p_MaterialId BIGINT,
    @p_UserId BIGINT
)
AS
BEGIN
    IF @p_OrderId IS NULL
    BEGIN
        INSERT INTO ProdOrderPart 
                   (CreationTime, CreatorUserId, IsDeleted, 
                    PartNo, PartName, SupplierNo, CarfamilyCode, 
                    Status, Remark, BaseUnitOfMeasure, MaterialId, 
                    Qty, AmountUnit, TotalAmount, OrderDate)
            VALUES (GETDATE(), @p_UserId, 0, 
                    @p_PartNo, @p_PartName, @p_SupplierNo, @p_Cfc,
                    @p_Status, @p_Remark, @p_BOM, @p_MaterialId,
                    @p_Qty, @p_AmountUnit, @p_TotalAmount, @p_OrderDate);

        SELECT @p_OrderId = @@identity;
    END
    ELSE
    BEGIN
        UPDATE ProdOrderPart
           SET LastModificationTime = GETDATE(),
               LastModifierUserId = @p_UserId,
               PartNo = @p_PartNo,
               PartName = @p_PartName,
               SupplierNo = @p_SupplierNo,
               CarfamilyCode = @p_Cfc,
               [Status] = @p_Status,
               Remark = @p_Remark,
               Qty = @p_Qty,
               AmountUnit = @p_AmountUnit,
               TotalAmount = @p_TotalAmount,
               OrderDate = @p_OrderDate,
               BaseUnitOfMeasure = @p_BOM,
               MaterialId = @p_MaterialId
         WHERE Id = @p_OrderId  
    END

    IF @p_Status = 'ORDER'
    BEGIN
          DECLARE @p_ContainerNo NVARCHAR(20) = 'CONT' + FORMAT(GETDATE(), 'yyMMddHHmmss');
          INSERT INTO ProdContainerIntransit 
                      (CreationTime, CreatorUserId, IsDeleted, 
                      ContainerNo, SupplierNo, Status, UsageQty, PartListId)
               VALUES (GETDATE(), @p_UserId, 0, 
                      @p_ContainerNo, @p_SupplierNo, 'NEW', @p_Qty, @p_OrderId);

          UPDATE ProdOrderPart
             SET ContainerNo = @p_ContainerNo
           WHERE Id = @p_OrderId
    END
END
------------------------------------------------DELETE:
CREATE OR ALTER PROCEDURE INV_PROD_ORDER_PART_DELETE
    @p_Id INT
AS
BEGIN
    DELETE ProdContainerIntransit WHERE PartListId = @p_Id

    UPDATE ProdOrderPart
       SET IsDeleted = 1
     WHERE Id = @p_Id
END
------------------------------------------------DASHBOARD------------------------------------------------
------------------------------------------------TOP
CREATE OR ALTER PROCEDURE INV_PROD_DASHBOARD_TOP 
    @p_Warehouse NVARCHAR(2)
AS
BEGIN
    DECLARE @New INT;
    DECLARE @OnPort INT;
    DECLARE @THC DECIMAL;
    DECLARE @CIF DECIMAL;
    DECLARE @TAX DECIMAL;
    DECLARE @VAT DECIMAL;
    DECLARE @Inventory INT;
    
    SELECT @New = COUNT(*) FROM ProdContainerIntransit pci WHERE pci.Status = 'NEW'
    SELECT @OnPort = COUNT(*) FROM ProdContainerIntransit pci WHERE pci.Status = 'PORT/ARRIVED' AND pci.IsDeleted = 0
    
    SELECT @THC = SUM(pid.Thc), @CIF = SUM(pid.Cif), @TAX = SUM(pid.Tax), @VAT = SUM(pid.Vat)
    FROM ProdInvoiceDetails pid 
    INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
    GROUP BY pid.InvoiceNo, pi.InvoiceDate
    ORDER BY pi.InvoiceDate
    
    DECLARE @Total DECIMAL = ISNULL(@CIF, 0) + ISNULL(@THC, 0) + ISNULL(@TAX, 0) + ISNULL(@VAT, 0);

    SELECT @Inventory = SUM(psr.ActualQty - ISNULL(psr.OrderedQty, 0)) 
    FROM ProdStockReceiving psr 
    WHERE (psr.ActualQty > psr.OrderedQty)
    AND psr.Warehouse = @p_Warehouse
    GROUP BY psr.Warehouse, psr.PartNo, psr.PartName, psr.Model
    
    SELECT ISNULL(@New, 0) NewCont, ISNULL(@OnPort, 0) ContOnPort, @Total TotalAmountInvoice, ISNULL(@Inventory, 0) Inventory
END
------------------------------------------------NewCont
CREATE OR ALTER PROCEDURE INV_PROD_DASHBOARD_NEW_CONT_TO_WAREHOUSE
    @p_Warehouse NVARCHAR(2)
AS
BEGIN
    SELECT TOP(5) pcrw.ContainerNo, pcrw.ReceiveDate  
      FROM ProdContainerRentalWHPlan pcrw
     WHERE pcrw.Warehouse = @p_Warehouse
  ORDER BY pcrw.ReceiveDate DESC
END
------------------------------------------------StockOut
CREATE OR ALTER PROCEDURE INV_PROD_DASHBOARD_STOCK_OUT
    @p_Warehouse NVARCHAR(2)
AS
BEGIN
    SELECT TOP(5) piso.ListCfc, piso.ListPartName, piso.TotalOrderQty, piso.TotalAmount, piso.InvoiceDate 
      FROM ProdInvoiceStockOut piso
INNER JOIN ProdStockReceiving psr ON psr.InvoiceNoOut LIKE CONCAT(piso.InvoiceNoOut, '%')
     WHERE psr.Warehouse = @p_Warehouse
       AND piso.InvoiceDate IS NOT NULL
  ORDER BY piso.InvoiceDate DESC
END
------------------------------------------------QtyOut
CREATE OR ALTER PROCEDURE INV_PROD_DASHBOARD_QTY_OUT
    @p_Type NVARCHAR(3),
    @p_InOrOut NVARCHAR(3),
    @p_DateFrom DATE,
    @p_DateTo DATE
AS
BEGIN
    IF @p_InOrOut = 'OUT'
    BEGIN
        IF @p_Type = 'Cfc'
        BEGIN
            SELECT piso.ListCfc Label, SUM(piso.TotalOrderQty) QtyOut
              FROM ProdInvoiceStockOut piso 
             WHERE piso.InvoiceDate IS NOT NULL 
               AND piso.InvoiceDate >= @p_DateFrom
               AND piso.InvoiceDate <= @p_DateTo
          GROUP BY piso.ListCfc
        END
        ELSE 
        BEGIN
            SELECT SUBSTRING(piso.GoodsDeliveryNoteNo, 1, 2) Label, SUM(piso.TotalOrderQty) QtyOut
              FROM ProdInvoiceStockOut piso 
             WHERE piso.InvoiceDate IS NOT NULL 
               AND piso.InvoiceDate >= @p_DateFrom
               AND piso.InvoiceDate <= @p_DateTo
          GROUP BY SUBSTRING(piso.GoodsDeliveryNoteNo, 1, 2)
        END
    END
    ELSE
    BEGIN
        IF @p_Type = 'Cfc'
        BEGIN
            SELECT psr.Model Label, SUM(psr.ActualQty) QtyOut
              FROM ProdStockReceiving psr
        INNER JOIN ProdContainerRentalWHPlan pcl on psr.ContainerNo = pcl.ContainerNo
             WHERE pcl.ReceiveDate >= @p_DateFrom
               AND pcl.ReceiveDate <= @p_DateTo
          GROUP BY psr.Model
        END
        ELSE 
        BEGIN
            SELECT psr.Warehouse Label, SUM(psr.ActualQty) QtyOut
              FROM ProdStockReceiving psr
        INNER JOIN ProdContainerRentalWHPlan pcl on psr.ContainerNo = pcl.ContainerNo
             WHERE pcl.ReceiveDate >= @p_DateFrom
               AND pcl.ReceiveDate <= @p_DateTo
             GROUP BY psr.Warehouse
        END
    END
END
------------------------------------------------INVOICE_STATISTICS
CREATE OR ALTER PROCEDURE INV_PROD_DASHBOARD_INVOICE_STATISTICS
    @p_DateFrom DATE,
    @p_DateTo DATE,
    @p_Type NVARCHAR(3)
AS
BEGIN
    IF @p_Type = 'IN'
    BEGIN
        SELECT pcrw.Warehouse, pi.InvoiceDate, SUM(pid.Cif) Cif, SUM(pid.Tax) Tax, SUM(pid.Vat) Vat
          FROM ProdInvoiceDetails pid
    INNER JOIN ProdInvoice pi ON pid.InvoiceNo = pi.InvoiceNo
    INNER JOIN ProdContainerRentalWHPlan pcrw ON pid.ContainerNo = pcrw.ContainerNo
         WHERE pi.InvoiceDate >= @p_DateFrom AND pi.InvoiceDate <= @p_DateTo
      GROUP BY pcrw.Warehouse, pi.InvoiceDate
      ORDER BY pi.InvoiceDate
    END
    ELSE
    BEGIN
        SELECT SUBSTRING(piso.GoodsDeliveryNoteNo, 1, 2) Warehouse, piso.InvoiceDate, SUM(piso.TotalAmount) AmountOut
          FROM ProdInvoiceStockOut piso
    INNER JOIN ProdStockReceiving psr 
            ON psr.InvoiceNoOut LIKE CONCAT(piso.InvoiceNoOut, '%')
         WHERE piso.InvoiceDate IS NOT NULL 
           AND piso.InvoiceDate >= @p_DateFrom 
           AND piso.InvoiceDate <= @p_DateTo
      GROUP BY SUBSTRING(piso.GoodsDeliveryNoteNo, 1, 2), piso.InvoiceDate
      ORDER BY piso.InvoiceDate
    END
END
------------------------------------------------Other(s)------------------------------------------------
CREATE TABLE ProcessLog (
  ID BIGINT IDENTITY
 ,CATEGORY VARCHAR(50) NULL
 ,PROCESS_NAME VARCHAR(200) NULL
 ,ERROR_MESSAGE VARCHAR(2048) NULL
 ,CREATED_BY NVARCHAR(40) NULL
 ,CREATED_DATE DATETIME NULL
)
GO
------------------------------------------------
CREATE FUNCTION fnSplit
(
    @sInputList VARCHAR(MAX), -- List of delimited items
    @sDelimiter VARCHAR(MAX) = ',' -- delimiter that separates items
) RETURNS @List TABLE (item VARCHAR(MAX))

BEGIN
    DECLARE @sItem VARCHAR(MAX)
      WHILE CHARINDEX(@sDelimiter, @sInputList, 0) <> 0
      BEGIN
            SELECT @sItem = RTRIM(LTRIM(SUBSTRING(@sInputList, 1, CHARINDEX(@sDelimiter, @sInputList, 0) - 1))),
                   @sInputList = RTRIM(LTRIM(SUBSTRING(@sInputList, 
                                 CHARINDEX(@sDelimiter, @sInputList, 0) + LEN(@sDelimiter), LEN(@sInputList))))
 
                IF LEN(@sItem) > 0
                   INSERT INTO @List SELECT @sItem
      END

    IF LEN(@sInputList) > 0
       INSERT INTO @List SELECT @sInputList -- Put the last item in

  RETURN
END

GO
------------------------------------------------
CREATE FUNCTION fn_DistinctList
(
    @String NVARCHAR(MAX),
    @Delimiter CHAR(1)
) RETURNS NVARCHAR(MAX)
WITH SCHEMABINDING
AS
BEGIN
    DECLARE @Result NVARCHAR(MAX);
    WITH MY_CTE AS ( SELECT DISTINCT(value) FROM STRING_SPLIT(@String, @Delimiter))
    SELECT @Result = STRING_AGG(value, @Delimiter) FROM MY_CTE
    RETURN @Result
END

GO