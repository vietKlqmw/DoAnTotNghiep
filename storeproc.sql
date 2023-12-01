------------------------------------------------Material------------------------------------------------
------------------------------------------------Search:
CREATE PROCEDURE INV_MASTER_MATERIAL_SEARCH
    @p_MaterialCode NVARCHAR(40),
    @p_MaterialGroup NVARCHAR(9)
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.Plant, mm.StorageLocation, mm.ProductionGroup, mm.ProductionType, 
           mm.ProductionPurpose, mm.ReservedStock, mm.LotCode, mm.ProductionStorageLocation, 
           mm.CostingLotSize, mm.ProductionVersion, mm.StandardPrice, mm.MovingPrice, 
           mm.MaterialOrigin, mm.OriginGroup, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE (@p_MaterialCode IS NULL OR mm.MaterialCode LIKE CONCAT('%', @p_MaterialCode, '%'))
       AND (@p_MaterialGroup IS NULL OR mm.MaterialGroup LIKE CONCAT('%', @p_MaterialGroup, '%'))
       AND mm.IsDeleted = 0
END
------------------------------------------------Get data by Id:
CREATE PROCEDURE INV_MASTER_MATERIAL_BY_ID
    @p_MaterialId BIGINT
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.Plant, mm.StorageLocation, mm.ProductionGroup, mm.ProductionType, 
           mm.ProductionPurpose, mm.ReservedStock, mm.LotCode, mm.ProductionStorageLocation, 
           mm.CostingLotSize, mm.ProductionVersion, mm.StandardPrice, mm.MovingPrice, 
           mm.MaterialOrigin, mm.OriginGroup, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE mm.Id = @p_MaterialId
END
------------------------------------------------Edit data:
CREATE PROCEDURE INV_MASTER_MATERIAL_EDIT
@p_MaterialId INT, 
@p_MaterialType NVARCHAR(4),
@p_MaterialCode NVARCHAR(40), 
@p_Description NVARCHAR(40), 
@p_MaterialGroup NVARCHAR(9), 
@p_BaseUnitOfMeasure NVARCHAR(3), 
@p_Plant NVARCHAR(4), 
@p_StorageLocation NVARCHAR(4), 
@p_ProductionGroup NVARCHAR(3), 
@p_ProductionPurpose NVARCHAR(2), 
@p_ProductionType NVARCHAR(10), 
@p_ReservedStock NVARCHAR(2), 
@p_LotCode NVARCHAR(10), 
@p_ProductionStorageLocation NVARCHAR(4), 
@p_CostingLotSize DECIMAL, 
@p_ProductionVersion NVARCHAR(4), 
@p_StandardPrice DECIMAL, 
@p_MovingPrice DECIMAL, 
@p_MaterialOrigin NVARCHAR(1), 
@p_OriginGroup NVARCHAR(4), 
@p_EffectiveDateFrom DATE, 
@p_EffectiveDateTo DATE, 
@p_UserId BIGINT
AS
BEGIN
    IF @p_MaterialId IS NULL
    BEGIN
        INSERT INTO MasterMaterial (CreationTime, CreatorUserId, IsDeleted, 
                                    MaterialType, MaterialCode, Description, MaterialGroup, BaseUnitOfMeasure, 
                                    Plant, StorageLocation, ProductionGroup, ProductionPurpose, ReservedStock, 
                                    LotCode, ProductionStorageLocation, CostingLotSize, ProductionVersion, 
                                    StandardPrice, MovingPrice, MaterialOrigin, OriginGroup, EffectiveDateFrom, 
                                    EffectiveDateTo, ProductionType)
                            VALUES (GETDATE(), @p_UserId, 0, 
                                    @p_MaterialType, @p_MaterialCode, @p_Description, @p_MaterialGroup, @p_BaseUnitOfMeasure, 
                                    @p_Plant, @p_StorageLocation, @p_ProductionGroup, @p_ProductionPurpose, @p_ReservedStock, 
                                    @p_LotCode, @p_ProductionStorageLocation, @p_CostingLotSize, @p_ProductionVersion, 
                                    @p_StandardPrice, @p_MovingPrice, @p_MaterialOrigin, @p_OriginGroup, @p_EffectiveDateFrom, 
                                    @p_EffectiveDateTo, @p_ProductionType);
    END
    ELSE
    BEGIN
        UPDATE MasterMaterial
           SET Description = @p_Description,
               BaseUnitOfMeasure = @p_BaseUnitOfMeasure,
               ReservedStock = @p_ReservedStock,
               CostingLotSize = @p_CostingLotSize,
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
CREATE PROCEDURE INV_MASTER_MATERIAL_DELETE
    @p_Id INT
AS
BEGIN
    UPDATE MasterMaterial
       SET IsDeleted = 1
     WHERE Id = @p_Id 
END
------------------------------------------------Import data:
CREATE PROCEDURE INV_MASTER_MATERIAL_MERGE
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
                   t.Plant, t.StorageLocation, t.ProductionGroup, t.ProductionPurpose, t.ReservedStock, 
                   t.LotCode, t.ProductionStorageLocation, t.CostingLotSize, t.ProductionVersion, 
                   t.StandardPrice, t.MovingPrice, t.MaterialOrigin, t.OriginGroup, t.EffectiveDateFrom, 
                   t.EffectiveDateTo, t.ProductionType, t.CreatorUserId
        		  FROM MasterMaterial_T t
        	   WHERE Guid = @Guid
		    ) AS DT	ON (P.MaterialType = DT.MaterialType AND P.MaterialCode = DT.MaterialCode)				    
		    WHEN MATCHED THEN
		        UPDATE SET 
					      P.Description = DT.Description,
					      P.MaterialGroup = DT.MaterialGroup,
					      P.BaseUnitOfMeasure = DT.BaseUnitOfMeasure,
					      P.Plant = DT.Plant, 
					      P.StorageLocation = DT.StorageLocation,
					      P.ProductionGroup = DT.ProductionGroup,
					      P.ProductionPurpose = DT.ProductionPurpose,
					      P.ReservedStock = DT.ReservedStock,
					      P.LotCode = DT.LotCode,
					      P.ProductionStorageLocation = DT.ProductionStorageLocation,
					      P.CostingLotSize = DT.CostingLotSize,
					      P.ProductionVersion = DT.ProductionVersion,
					      P.StandardPrice = DT.StandardPrice,
                P.MovingPrice = DT.MovingPrice,
                P.MaterialOrigin = DT.MaterialOrigin,
                P.OriginGroup = DT.OriginGroup,
                P.EffectiveDateFrom = DT.EffectiveDateFrom,
                P.EffectiveDateTo = DT.EffectiveDateTo,
                P.ProductionType = DT.ProductionType,
                P.LastModifierUserId = DT.CreatorUserId,
					      P.LastModificationTime = GETDATE()
		    WHEN NOT MATCHED THEN
		        INSERT (CreationTime, CreatorUserId, IsDeleted, 
                    MaterialType, MaterialCode, Description, MaterialGroup, BaseUnitOfMeasure, 
                    Plant, StorageLocation, ProductionGroup, ProductionPurpose, ReservedStock, 
                    LotCode, ProductionStorageLocation, CostingLotSize, ProductionVersion, 
                    StandardPrice, MovingPrice, MaterialOrigin, OriginGroup, EffectiveDateFrom, 
                    EffectiveDateTo, ProductionType)				
		  	    VALUES (GETDATE(), DT.CreatorUserId, 0, 
                    DT.MaterialType, DT.MaterialCode, DT.Description, DT.MaterialGroup, DT.BaseUnitOfMeasure, 
                    DT.Plant, DT.StorageLocation, DT.ProductionGroup, DT.ProductionPurpose, DT.ReservedStock, 
                    DT.LotCode, DT.ProductionStorageLocation, DT.CostingLotSize, DT.ProductionVersion, 
                    DT.StandardPrice, DT.MovingPrice, DT.MaterialOrigin, DT.OriginGroup, DT.EffectiveDateFrom, 
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
CREATE PROCEDURE INV_MASTER_MATERIAL_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(max)
AS 
BEGIN
    SELECT DISTINCT mmt.Guid, mmt.MaterialType, mmt.MaterialCode, mmt.Description, mmt.MaterialGroup, 
           mmt.BaseUnitOfMeasure, mmt.Plant, mmt.StorageLocation, mmt.ProductionGroup, mmt.ProductionType, 
           mmt.ProductionPurpose, mmt.ReservedStock, mmt.LotCode, mmt.ProductionStorageLocation, 
           mmt.CostingLotSize, mmt.ProductionVersion, mmt.StandardPrice, mmt.MovingPrice, 
           mmt.MaterialOrigin, mmt.OriginGroup, mmt.EffectiveDateFrom, mmt.EffectiveDateTo,
           mmt.ErrorDescription
      FROM MasterMaterial_T mmt
     WHERE mmt.Guid = @Guid AND ISNULL(mmt.ErrorDescription, '') <> '' 
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
------------------------------------------------ProductGroup------------------------------------------------
INSERT INTO MasterProductGroup 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'PC', N'PC'),
(GETDATE(), 1, 0, N'CV', N'CV');
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
------------------------------------------------Factory------------------------------------------------
INSERT INTO MasterFactory 
(CreationTime, CreatorUserId, IsDeleted, 
PlantCode, PlantName, BranchNo, AddressLanguageEn, AddressLanguageVn)
VALUES 
(GETDATE(), 1, 0, 
N'451', N'VMV HQ', N'', 
N'Phuc Thang ward – Phuc Yen city – Vinh Phuc province', 
N'Viet Nam / Phường Phúc Thắng – Thành phố Phúc Yên – Tỉnh Vĩnh Phúc – Việt Nam'),
(GETDATE(), 1, 0, 
N'452', N'VMV Trading Plant', N'', 
N'Phuc Thang ward – Phuc Yen city – Vinh Phuc province', 
N'Viet Nam / Phường Phúc Thắng – Thành phố Phúc Yên – Tỉnh Vĩnh Phúc – Việt Nam');
------------------------------------------------StorageLocation------------------------------------------------
INSERT INTO MasterStorageLocation 
(CreationTime, CreatorUserId, IsDeleted, 
PlantCode, PlantName, StorageLocation, StorageLocationName, 
AddressLanguageEn, AddressLanguageVn, Category)
VALUES 
(GETDATE(), 1, 0, 
N'451', N'VMV', N'101', N'GWH (CKD part)', 
N'Phuc Thang ward - Phuc Yen city - Vinh Phuc province - Viet Nam', 
N'Phường Phúc Thắng - Thành phố Phúc Yên - Tỉnh Vĩnh Phúc - Việt Nam', 
N'Raw Material'),
(GETDATE(), 1, 0, 
N'451', N'VMV', N'102', N'ACC for CBU', 
N'Phuc Thang ward - Phuc Yen city - Vinh Phuc province - Viet Nam', 
N'Phường Phúc Thắng - Thành phố Phúc Yên - Tỉnh Vĩnh Phúc - Việt Nam', 
N'Raw Material'),
(GETDATE(), 1, 0, 
N'451', N'VMV', N'301', N'Production Area', 
N'Phuc Thang ward - Phuc Yen city - Vinh Phuc province - Viet Nam', 
N'Phường Phúc Thắng - Thành phố Phúc Yên - Tỉnh Vĩnh Phúc - Việt Nam', 
N'Production Area'),
(GETDATE(), 1, 0, 
N'451', N'VMV', N'401', N'Vehicle Yard', 
N'Phuc Thang ward - Phuc Yen city - Vinh Phuc province - Viet Nam', 
N'Phường Phúc Thắng - Thành phố Phúc Yên - Tỉnh Vĩnh Phúc - Việt Nam', 
N'Finished Goods');
--Search
CREATE PROCEDURE INV_MASTER_STORAGE_LOCATION_SEARCH
    @p_PlantName NVARCHAR(30),
    @p_StorageLocationName NVARCHAR(MAX),
    @p_AddressLanguageEn NVARCHAR(MAX),
    @p_Category NVARCHAR(50)
AS 
BEGIN
    SELECT msl.Id, msl.PlantCode, msl.PlantName, msl.StorageLocation, msl.StorageLocationName, 
           msl.AddressLanguageEn, msl.AddressLanguageVn, msl.Category
      FROM MasterStorageLocation msl
     WHERE (@p_PlantName IS NULL OR msl.PlantName LIKE CONCAT('%', @p_PlantName, '%'))
       AND (@p_StorageLocationName IS NULL OR msl.StorageLocationName LIKE CONCAT('%', @p_StorageLocationName, '%'))
       AND (@p_AddressLanguageEn IS NULL OR msl.AddressLanguageEn LIKE CONCAT('%', @p_AddressLanguageEn, '%'))
       AND (@p_Category IS NULL OR msl.Category LIKE CONCAT('%', @p_Category, '%'))
       AND msl.IsDeleted = 0
END
------------------------------------------------InvoiceStatus------------------------------------------------
INSERT INTO MasterInvoiceStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description)
VALUES 
(GETDATE(), 1, 0, N'IS01', N'BEING PRE CUSTOMS'),
(GETDATE(), 1, 0, N'IS02', N'PRE CUSTOMS'),
(GETDATE(), 1, 0, N'IS03', N'BEING CUSTOMS'),
(GETDATE(), 1, 0, N'IS04', N'CUSTOMS DECLARED'),
(GETDATE(), 1, 0, N'IS05', N'NEW'),
(GETDATE(), 1, 0, N'IS06', N'ALLOCATING'),
(GETDATE(), 1, 0, N'IS07', N'ALLOCATED FI'),
(GETDATE(), 1, 0, N'IS08', N'ALLOCATED')
;
------------------------------------------------CustomsStatus------------------------------------------------
INSERT INTO MasterCustomsStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description)
VALUES 
(GETDATE(), 1, 0, N'CuS1', N'NEW'),
(GETDATE(), 1, 0, N'CuS2', N'COMPLETED'),
(GETDATE(), 1, 0, N'CuS3', N'NOT PAID (REQUESTED)'),
(GETDATE(), 1, 0, N'CuS4', N'PAID');
------------------------------------------------ContainerStatus------------------------------------------------
INSERT INTO MasterContainerStatus 
(CreationTime, CreatorUserId, IsDeleted, Code, Description, DescriptionVn)
VALUES 
(GETDATE(), 1, 0, N'CS01', N'ON SEA', N'TRÊN BIỂN'),
(GETDATE(), 1, 0, N'CS02', N'PORT/ARRIVED (As ETA)', N'Ở CẢNG'),
(GETDATE(), 1, 0, N'CS03', N'PORT/ARRIVED (As ATA)', N'Ở CẢNG'),
(GETDATE(), 1, 0, N'CS04', N'TRANSFERRING', N'ĐANG ĐI ĐƯỜNG'),
(GETDATE(), 1, 0, N'CS05', N'YARD (WAITING)', N'BÃI CHỜ'),
(GETDATE(), 1, 0, N'CS06', N'DEVANNING', N'ĐÃ DỠ'),
(GETDATE(), 1, 0, N'CS07', N'RETURN CASE', N'ĐÓNG VỎ THÙNG HÀNG'),
(GETDATE(), 1, 0, N'CS08', N'RETURN CBU', N'CBU'),
(GETDATE(), 1, 0, N'CS09', N'EXPORT', N'XUẤT KHẨU'),
(GETDATE(), 1, 0, N'CS10', N'NEW', N'MỚI')
;
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
------------------------------------------------Shipment------------------------------------------------
CREATE PROCEDURE INV_PROD_SHIPMENT_SEARCH
(
    @p_ShipmentNo NVARCHAR(10),
    @p_SupplierNo NVARCHAR(10),
    @p_FromPort NVARCHAR(50),
    @p_ToPort NVARCHAR(50),
    @p_ShipmentDate DATE
)
AS 
   SELECT DISTINCT a.Id, a.ShipmentNo, a.SupplierNo,
          a.Buyer, a.FromPort, a.ToPort, a.ShipmentDate,
          a.Etd, a.Eta, a.Ata, a.OceanVesselName, a.Atd, a.Status
     FROM ProdShipment a
    WHERE (@p_ShipmentNo IS NULL OR a.ShipmentNo LIKE CONCAT('%', @p_ShipmentNo, '%'))
      AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
      AND (@p_FromPort IS NULL OR a.FromPort LIKE CONCAT('%', @p_FromPort, '%'))
      AND (@p_ToPort IS NULL OR a.ToPort LIKE CONCAT('%', @p_ToPort, '%'))
      AND (@p_ShipmentDate IS NULL OR a.ShipmentDate = @p_ShipmentDate)
      AND a.IsDeleted = 0
 ORDER BY a.Etd DESC, a.Eta DESC
------------------------------------------------BillOfLading------------------------------------------------
CREATE PROCEDURE INV_PROD_BILL_OF_LADING_SEARCH
(
    @p_BillofladingNo NVARCHAR(20),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE
)
AS 
    SELECT DISTINCT b.Id, b.BillofladingNo, b.ShipmentId, b.BillDate, b.StatusCode
      FROM ProdBillOfLading b 
 LEFT JOIN ProdInvoice c 
        ON c.BillId = b.Id
     WHERE (@p_BillofladingNo IS NULL OR b.BillofladingNo LIKE CONCAT('%', @p_BillofladingNo, '%'))
       AND (@p_BillDateFrom IS NULL OR b.BillDate >= @p_BillDateFrom)
       AND (@p_BillDateTo IS NULL OR b.BillDate <= @p_BillDateTo)
       AND b.IsDeleted = 0
  ORDER BY b.BillDate DESC
------------------------------------------------Invoice------------------------------------------------
CREATE PROCEDURE INV_PROD_INVOICE_SEARCH 
(
    @p_InvoiceNo NVARCHAR(20),
    @p_InvoiceDateFrom DATE,
    @p_InvoiceDateTo DATE,
    @p_BillNo NVARCHAR(20),
    @p_ShipmentNo NVARCHAR(10),
    @p_ContainerNo NVARCHAR(15),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE,
    @p_SupplierNo NVARCHAR(10)
)
AS
BEGIN 
    SELECT DISTINCT a.Id, a.InvoiceNo, a.BillId, a.OrderTypeCode, a.GoodsTypeCode, a.InvoiceDate, 
           a.Freight, a.FreightTotal, a.Insurance, a.InsuranceTotal, a.Cif, a.ThcTotal, a.NetWeight, 
           a.GrossWeight, a.Currency, a.SupplierNo, a.Quantity, a.FreightTotalVn, a.InsuranceTotalVn, 
           a.CifVn, a.ThcTotalVn, a.PeriodId, b.BillofladingNo AS BillNo, c.ShipmentNo, 
           e.Description AS Status, parent.InvoiceNo InvoiceParentId, b.BillDate
      FROM ProdInvoice a
 LEFT JOIN ProdBillOfLading b
        ON a.BillId = b.Id
 LEFT JOIN ProdShipment c
        ON c.Id = b.ShipmentId
 LEFT JOIN ProdContainerInvoice d
        ON d.InvoiceId = a.Id
 LEFT JOIN ProdInvoice parent
        ON a.InvoiceParentId = parent.Id
	LEFT JOIN MasterInvoiceStatus e
        ON a.Status = e.Code
     WHERE (@p_InvoiceNo IS NULL OR a.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (@p_InvoiceDateFrom IS NULL OR a.InvoiceDate >= @p_InvoiceDateFrom)
       AND (@p_InvoiceDateTo IS NULL OR a.InvoiceDate <= @p_InvoiceDateTo)
       AND (@p_BillNo IS NULL OR b.BillofladingNo LIKE CONCAT('%', @p_BillNo, '%'))
       AND (@p_ShipmentNo IS NULL OR c.ShipmentNo LIKE CONCAT('%', @p_ShipmentNo, '%'))
       AND (@p_ContainerNo IS NULL OR d.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_BillDateFrom IS NULL OR b.BillDate >= @p_BillDateFrom)
       AND (@p_BillDateTo IS NULL OR b.BillDate < DATEADD(DAY, 1, @p_BillDateTo))
	     AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND a.IsDeleted = 0
  ORDER BY a.InvoiceDate DESC, b.BillDate DESC, a.InvoiceNo, a.Id	
END
------------------------------------------------InvoiceDetails------------------------------------------------
CREATE PROCEDURE INV_PROD_INVOICE_DETAILS_SEARCH
(
    @p_InvoiceId INT
)
AS 
BEGIN
    SELECT a.PartNo, a.LotNo, a.Fixlot, a.CaseNo, a.ModuleNo, a.Insurance, a.ContainerNo, a.InvoiceId, 
           a.SupplierNo, a.Freight, a.Thc, a.Cif, a.Tax, a.TaxRate, a.Vat, a.VatRate, a.UsageQty, 
           a.PartName, a.CarfamilyCode, a.PartNetWeight, a.OrderNo, a.PackagingDate, a.FreightVn, 
           a.InsuranceVn, a.ThcVn, a.CifVn, a.TaxVn, a.VatVn, a.InvoiceParentId, a.PeriodDate, 
           a.PeriodId, a.PartnameVn, a.CarName, a.PreCustomsId--, e.Description AS Status
      FROM ProdInvoiceDetails a 
 LEFT JOIN ProdInvoice inv 
        ON inv.Id = CASE WHEN inv.Id = inv.InvoiceParentId THEN a.InvoiceParentId ELSE a.InvoiceId END
 LEFT JOIN ProdContainerInvoice d 
        ON d.InvoiceId = inv.InvoiceParentId AND d.ContainerNo = a.ContainerNo
-- LEFT JOIN MasterInvoiceStatus e
--        ON a.Status = e.Code
     WHERE inv.Id = @p_InvoiceId
       AND a.IsDeleted = 0
  ORDER BY a.PartNo
END
------------------------------------------------ContainerInvoice------------------------------------------------
CREATE PROCEDURE INV_PROD_CONTAINER_INVOICE_SEARCH
(
    @p_BillNo NVARCHAR(20),
    @p_ContainerNo NVARCHAR(15),
    @p_InvoiceNo NVARCHAR(20),
    @p_SealNo NVARCHAR(20),
    @p_Status NVARCHAR(50),
    @p_SupplierNo NVARCHAR(10),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE
)
AS 
    SELECT a.Id, a.ContainerNo, a.InvoiceId, a.SupplierNo, a.SealNo, a.ContainerSize, 
           a.PlanDevanningDate, a.ActualDevanningDate, a.Thc, b.Description Status, 
           a.ThcVn, a.Freight, a.Insurance, a.Tax, a.Amount, a.TaxVnd, a.VatVnd, 
           c.BillofladingNo, c.BillDate, d.InvoiceNo
      FROM ProdContainerInvoice a
INNER JOIN ProdInvoice d     
        ON d.Id = a.InvoiceId
INNER JOIN ProdBillOfLading c 
        ON c.Id = d.BillId
 LEFT JOIN MasterCustomsStatus b
        ON a.Status = b.Code
     WHERE (@p_BillNo IS NULL OR c.BillofladingNo LIKE CONCAT('%', @p_BillNo, '%'))
	     AND (@p_ContainerNo IS NULL OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_InvoiceNo IS NULL OR d.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (@p_SealNo IS NULL OR a.SealNo LIKE CONCAT('%', @p_SealNo, '%'))
       AND (@p_Status = '-1' OR a.Status = @p_Status)
       AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_BillDateFrom IS NULL OR c.BillDate >= @p_BillDateFrom)
       AND (@p_BillDateTo IS NULL OR c.BillDate <= @p_BillDateTo)
       AND a.IsDeleted = 0
  ORDER BY c.BillDate DESC, a.PlanDevanningDate DESC, b.Description

GO
------------------------------------------------ContainerList------------------------------------------------
CREATE PROCEDURE INV_PROD_CONTAINER_LIST_SEARCH 
(
	  @p_ContainerNo NVARCHAR(15),
	  @p_SupplierNo NVARCHAR(10),
	  @p_BillOfLadingNo NVARCHAR(20),
	  @p_PortDateFrom DATE,
	  @p_PortDateTo DATE,
	  @p_ReceiveDateFrom DATE,
	  @p_ReceiveDateTo DATE,
	  @p_InvoiceNo NVARCHAR(20),
	  @p_BillDateFrom DATE,
	  @p_BillDateTo DATE,
	  @p_ContainerStatus VARCHAR(1)
)
AS
BEGIN 
    SELECT DISTINCT a.Id,
                    CASE WHEN a.RequestStatus = 0 THEN 'New'
                         WHEN a.RequestStatus = 1 THEN 'Firmed'
                         WHEN a.RequestStatus = 2 THEN 'Canceled'
                         WHEN a.RequestStatus = 3 THEN 'Pending'
                    ELSE a.RequestStatus END RequestStatus,
			              a.ContainerNo, a.SupplierNo, a.BillOfLadingNo, a.SealNo, a.ContainerSize, 
                    a.ShipmentId, a.ShippingDate, a.PortDate, a.PortDateActual, a.PortTransitDate, 
			              a.ReceiveDate, a.RequestId, a.InvoiceNo, a.ListLotNo, a.ListCaseNo, a.Transport, 
			              a.DevanningDate, a.DevanningTime, a.Remark, a.WhLocation, a.GateInDate, 
			              a.GateInTime, a.TransitPortReqId, a.TransitPortReqDate, a.TransitPortReqTime, 
			              a.Freight, a.Insurance, a.Cif, a.Tax, a.Amount, b.Description Status, 
			              a.LocationCode, a.LocationDate, a.ReceivingPeriodId, a.OrdertypeCode, 
			              a.GoodstypeCode, a.BillDate, a.ReceiveTime
               FROM ProdContainerList a
         INNER JOIN ProdContainerInvoice c
                 ON c.ContainerNo = a.ContainerNo 
         INNER JOIN ProdInvoice d 
                 ON d.Id = c.InvoiceId
	        LEFT JOIN MasterContainerStatus b
                 ON a.Status = b.Code
              WHERE (@p_ContainerNo IS NULL OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%') 
                     OR a.ContainerNo IN (SELECT item FROM dbo.fnSplit(@p_ContainerNo, ';')) 
                    )
    		        AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
    		        AND (@p_BillOfLadingNo IS NULL OR a.BillOfLadingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
                AND (@p_PortDateFrom IS NULL OR a.PortDate >= @p_PortDateFrom)
                AND (@p_PortDateTo IS NULL OR a.PortDate <= @p_PortDateTo)
                AND (@p_ReceiveDateFrom IS NULL OR a.ReceiveDate >= @p_ReceiveDateFrom)
                AND (@p_ContainerStatus IN ('1','2') OR @p_ReceiveDateTo IS NULL OR a.ReceiveDate <= @p_ReceiveDateTo)
                AND (@p_InvoiceNo IS NULL OR a.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
                AND (@p_BillDateFrom IS NULL OR a.BillDate >= @p_BillDateFrom)
                AND (@p_BillDateTo IS NULL OR a.BillDate <= @p_BillDateTo)		  
			  -- Xoay quanh tham số Receive Date To, để xác định intransit hay không, dựa vào các điều kiện dưới đây
			  -- 1. Nếu không nhập -> Intransit là các invoice được nhận về
			  -- 2. Nếu nhập, invoice intransit phải là các invoice chưa được nhận về TMV hoặc nhận về TMV sau thời điểm receive date to. Nhưng bắt buộc phải là invoice có bill date trước ngày Receive Date to
    		        AND (@p_ContainerStatus IS NULL 
                     OR (@p_ContainerStatus = '1' 
                         AND ((@p_ReceiveDateTo IS NULL AND a.ReceiveDate IS NULL) 
                              OR (@p_ReceiveDateTo IS NOT NULL AND ISNULL(a.ReceiveDate, '2999-12-31') > @p_ReceiveDateTo)
                             )-- 1.
                         AND ((@p_ReceiveDateTo IS NULL) 
                              OR (@p_ReceiveDateTo IS NOT NULL AND ISNULL(a.BillDate, d.InvoiceDate) < @p_ReceiveDateTo)
                             )-- 2.trang thai intransit truoc ngay ve tmv
                    ) 
    						     OR (@p_ContainerStatus = '2' 
                         AND ((@p_ReceiveDateTo IS NULL AND a.ReceiveDate IS NULL) 
                              OR (@p_ReceiveDateTo IS NOT NULL AND ISNULL(a.ReceiveDate, '2999-12-31') > @p_ReceiveDateTo)
                             ) 
                         AND a.TransitPortReqId IS NOT NULL) -- Cont ở tại intransit port trước ngày về TMV
    						     OR (@p_ContainerStatus = '3' AND a.ReceiveDate IS NOT NULL)
    						     OR (@p_ContainerStatus = '4' AND a.ReceiveDate IS NOT NULL AND a.RentalWhId IS NOT NULL)
    			          )
                AND a.IsDeleted = 0
           ORDER BY a.ShippingDate DESC, a.PortDate DESC, a.ReceiveDate DESC
END
------------------------------------------------ContainerWarehouse------------------------------------------------
------------------------------------------------search:
CREATE PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_SEARCH
(
    @p_ContainerNo NVARCHAR(15),
    @p_InvoiceNo NVARCHAR(20),
    @p_BillOfLadingNo NVARCHAR(20),
    @p_SupplierNo NVARCHAR(10),
    @p_SealNo NVARCHAR(20),
    @p_RequestDateFrom DATE,
    @p_RequestDateTo DATE
)
AS
BEGIN 
    SELECT a.Id, a.ContainerNo, a.RequestDate, a.RequestTime, a.InvoiceNo, a.BillofladingNo, 
           a.SupplierNo, a.SealNo, a.ListcaseNo, a.ListLotNo, a.DevanningDate, a.DevanningTime, 
           a.ActualDevanningDate, a.GateInPlanTime, a.GateInActualDateTime, a.Transport, a.Status
      FROM ProdContainerRentalWHPlan a
     WHERE (@p_ContainerNo IS NULL OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (@p_InvoiceNo IS NULL OR a.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (@p_BillOfLadingNo IS NULL OR a.BillofladingNo LIKE CONCAT('%', @p_BillOfLadingNo, '%'))
       AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (@p_SealNo IS NULL OR a.SealNo LIKE CONCAT('%', @p_SealNo, '%'))
       AND (@p_RequestDateFrom IS NULL OR a.RequestDate >= @p_RequestDateFrom)
       AND (@p_RequestDateTo IS NULL OR a.RequestDate <= @p_RequestDateTo)
       AND a.IsDeleted = 0
  ORDER BY a.RequestDate DESC
END
------------------------------------------------edit:
CREATE PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_EDIT
@p_Id INT, 
@p_ContainerNo NVARCHAR(15),
@p_RequestDate DATE, 
@p_RequestTime TIME, 
@p_InvoiceNo NVARCHAR(20), 
@p_BillOfLadingNo NVARCHAR(20), 
@p_SupplierNo NVARCHAR(10), 
@p_SealNo NVARCHAR(20), 
@p_ListcaseNo NVARCHAR(MAX), 
@p_ListLotNo NVARCHAR(MAX), 
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
                     SupplierNo, SealNo, ListcaseNo, ListLotNo, DevanningDate, 
                     DevanningTime, ActualDevanningDate, GateInPlanTime, 
                     GateInActualDateTime, Transport, Status)
             VALUES (GETDATE(), @p_UserId, 0, 
                     @p_ContainerNo, @p_RequestDate, @p_RequestTime, @p_InvoiceNo, @p_BillOfLadingNo, 
                     @p_SupplierNo, @p_SealNo, @p_ListcaseNo, @p_ListLotNo, @p_DevanningDate, 
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
               ListcaseNo = @p_ListcaseNo,
               ListLotNo = @p_ListLotNo,
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
------------------------------------------------delete:
CREATE PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_DELETE
    @p_Id INT
AS
BEGIN
    UPDATE ProdContainerRentalWHPlan
       SET IsDeleted = 1
     WHERE Id = @p_Id 
END
------------------------------------------------Import:
CREATE PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_MERGE
    @Guid VARCHAR(MAX)
AS
BEGIN
    BEGIN TRY 
	  BEGIN TRANSACTION

	  -- FOR BUG CHECK-
		INSERT INTO ProcessLog(CATEGORY, PROCESS_NAME, ERROR_MESSAGE, CREATED_BY, CREATED_DATE)
		VALUES ('ProdContainerRentalWHPlan', 'ProdContainerRentalWHPlan Import', 'START:', 'SYSTEM', GETDATE());

		UPDATE t1 
		   SET ErrorDescription = CONCAT(ISNULL(ErrorDescription, ''), 'Không tồn tại Container: ', t1.ContainerNo)
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
                 MAX(t2.SupplierNo) SupplierNo, t1.SealNo, t1.ListcaseNo,
                 t1.DevanningDate, t1.DevanningTime, t1.Transport, t1.CreatorUserId
        		FROM ProdContainerRentalWHPlan_T t1
      INNER JOIN ProdContainerInvoice t2 ON t1.ContainerNo = t2.ContainerNo
      INNER JOIN ProdInvoice t3 ON t3.Id = t2.InvoiceId
      INNER JOIN ProdBillOfLading t4 ON t3.BillId = t4.Id
        	 WHERE Guid = @Guid 
        GROUP BY t1.ContainerNo, t1.SealNo, t1.ListcaseNo, t1.DevanningDate, t1.DevanningTime, t1.Transport, t1.CreatorUserId
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
					P.ListcaseNo = DT.ListcaseNo,
					P.Transport = DT.Transport,
					P.SupplierNo = DT.SupplierNo,
          P.LastModifierUserId = DT.CreatorUserId,
					P.LastModificationTime = GETDATE()
		WHEN NOT MATCHED THEN
		    INSERT (
                ContainerNo, InvoiceNo, BillofladingNo, SupplierNo, SealNo, 
                ListcaseNo, DevanningDate, DevanningTime, Status, Transport,
                RequestDate, RequestTime, 
                CreationTime, CreatorUserId, IsDeleted)				
		  	VALUES (
                ContainerNo, InvoiceNo, BillofladingNo, SupplierNo, SealNo,
                ListcaseNo, DT.DevanningDate, DevanningTime, 'R', Transport,
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
CREATE PROCEDURE INV_PROD_CONTAINER_WAREHOUSE_GET_LIST_ERROR_IMPORT
    @Guid NVARCHAR(MAX)
AS 
BEGIN
    SELECT DISTINCT Guid, ContainerNo, InvoiceNo, BillofladingNo, 
           SealNo, ListcaseNo, DevanningDate, DevanningTime, 
           Transport, ErrorDescription
      FROM ProdContainerRentalWHPlan_T
     WHERE Guid = @Guid AND ISNULL(ErrorDescription, '') <> '' 
END
GO
------------------------------------------------ContainerIntransit------------------------------------------------
CREATE PROCEDURE INV_PROD_CONTAINER_INTRANSIT_SEARCH
(
	  @p_ContainerNo NVARCHAR(15),
	  @p_ShippingDate DATE,
	  @p_PortDate DATE,
	  @p_TransactionDate DATE,
	  @p_TmvDate DATE
)
AS
    SELECT a.Id, a.ContainerNo, a.SupplierNo, a.ShippingDate, a.PortDate, 
           a.TransactionDate, a.TmvDate, b.Description AS Status, a.Forwarder
      FROM ProdContainerIntransit a
 LEFT JOIN MasterContainerStatus b
        ON a.Status = b.Code
     WHERE (ISNULL(@p_ContainerNo, '') = '' OR a.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
    	 AND (ISNULL(@p_ShippingDate, '') = '' OR a.ShippingDate = @p_ShippingDate)
    	 AND (ISNULL(@p_PortDate, '') = '' OR a.PortDate = @p_PortDate)
    	 AND (ISNULL(@p_TransactionDate, '') = '' OR a.TransactionDate = @p_TransactionDate)
    	 AND (ISNULL(@p_TmvDate, '') = '' OR a.TmvDate = @p_TmvDate)
		   AND a.IsDeleted = 0
	ORDER BY a.ShippingDate DESC

GO
------------------------------------------------StockReceving------------------------------------------------
CREATE PROCEDURE INV_PROD_STOCK_RECEIVING_SEARCH 
(
	  @p_PartNo NVARCHAR(12),
	  @p_WorkingDateFrom DATE,
	  @p_WorkingDateTo DATE,
    @p_SupplierNo NVARCHAR(15),
	  @p_ContainerNo NVARCHAR(15),
	  @p_InvoiceNo NVARCHAR(20),
    @p_Model NVARCHAR(4)
)
AS
BEGIN
	  SELECT r.Id, r.PartNo, r.PartName, r.PartListId, r.PartListGradeId, r.MaterialId, 
           ISNULL(r.Qty, 0) Qty, r.TransactionDatetime, r.InvoiceDetailsId, r.WorkingDate, 
		       r.SupplierNo, d.ContainerNo, d.InvoiceNo, r.Model
      FROM ProdStockReceiving r
INNER JOIN ProdInvoiceDetails d
       	ON d.Id = r.InvoiceDetailsId
	   WHERE (ISNULL(@p_PartNo, '') = '' OR r.PartNo LIKE CONCAT('%', @p_PartNo, '%'))
		   AND (ISNULL(@p_WorkingDateFrom, '')= '' OR  r.WorkingDate >= @p_WorkingDateFrom)
       AND (ISNULL(@p_WorkingDateTo, '')= '' OR r.WorkingDate <= @p_WorkingDateTo)
       AND (ISNULL(@p_SupplierNo, '') = '' OR r.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
       AND (ISNULL(@p_ContainerNo, '') = '' OR d.ContainerNo LIKE CONCAT('%', @p_ContainerNo, '%'))
       AND (ISNULL(@p_InvoiceNo, '') = '' OR d.InvoiceNo LIKE CONCAT('%', @p_InvoiceNo, '%'))
       AND (ISNULL(@p_Model, '') = '' OR r.Model LIKE CONCAT('%', @p_Model, '%'))
       AND r.IsDeleted = 0
  ORDER BY r.Model, r.WorkingDate DESC, r.PartNo
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