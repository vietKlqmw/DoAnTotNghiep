﻿------------------------------------------------Material------------------------------------------------
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
------------------------------------------------FuelType------------------------------------------------
INSERT INTO MasterFuelType 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, N'F1', N'Diesel'),
(GETDATE(), 1, 0, N'F2', N'Gasoline'),
(GETDATE(), 1, 0, N'F3', N'Hybrid'),
(GETDATE(), 1, 0, N'F4', N'Electronic');
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
------------------------------------------------CarSeries------------------------------------------------
INSERT INTO MasterCarSeries 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, 'P001', 'Camry'),
(GETDATE(), 1, 0, 'P002', 'Corolla'),
(GETDATE(), 1, 0, 'P003', 'Vios'),
(GETDATE(), 1, 0, 'C001', 'Innova'),
(GETDATE(), 1, 0, 'C002', 'Alphard'),
(GETDATE(), 1, 0, 'C003', 'Lexus'),
(GETDATE(), 1, 0, 'C004', 'Coaster'),
(GETDATE(), 1, 0, 'C005', 'Ventury'),
(GETDATE(), 1, 0, 'C006', 'Rush');
------------------------------------------------TransmissionType------------------------------------------------
INSERT INTO MasterTransmissionType 
(CreationTime, CreatorUserId, IsDeleted, Code, Name)
VALUES 
(GETDATE(), 1, 0, 'AT', 'Automatic'),
(GETDATE(), 1, 0, 'MT', 'Manual'),
(GETDATE(), 1, 0, 'HP', 'Hibrid');
------------------------------------------------EngineType------------------------------------------------

------------------------------------------------EngineModel------------------------------------------------

------------------------------------------------VehicleCBU------------------------------------------------
CREATE PROCEDURE INV_MASTER_VEHICLE_CBU_SEARCH
    @p_VehicleType NVARCHAR(10),
    @p_Model NVARCHAR(MAX)
AS
BEGIN
    SELECT mvc.Id, mvc.VehicleType, mvc.Model, mvc.MarketingCode, mvc.ProductionCode 
      FROM MasterVehicleCBU mvc
     WHERE (@p_VehicleType IS NULL OR mvc.VehicleType LIKE CONCAT('%',@p_VehicleType,'%'))
       AND (@p_Model IS NULL OR mvc.Model LIKE CONCAT('%',@p_Model,'%'))
       AND mvc.IsDeleted = 0
END
------------------------------------------------VehicleCKD------------------------------------------------
CREATE PROCEDURE INV_MASTER_VEHICLE_CKD_SEARCH
    @p_Model NVARCHAR(1),
    @p_Cfc NVARCHAR(4),
    @p_ModelCode NVARCHAR(50)
AS
BEGIN
    SELECT mvc.Id, mvc.Model, mvc.LotCode, mvc.Cfc, mvc.Grade, mvc.GradeName, mvc.ModelCode, 
           mvc.VehicleId, mvc.CarSeries, mvc.TransmissionType, mvc.EngineType, mvc.FuelType
      FROM MasterVehicleCKD mvc
     WHERE (@p_Model IS NULL OR mvc.Model LIKE CONCAT('%', @p_Model, '%'))
       AND (@p_Cfc IS NULL OR mvc.Cfc LIKE CONCAT('%', @p_Cfc, '%'))
       AND (@p_ModelCode IS NULL OR mvc.ModelCode LIKE CONCAT('%', @p_ModelCode, '%'))
       AND mvc.IsDeleted = 0
END
------------------------------------------------Engine------------------------------------------------
CREATE PROCEDURE INV_MASTER_ENGINE_SEARCH
    @p_MaterialCode NVARCHAR(40),
    @p_TransmissionType NVARCHAR(5),
    @p_EngineModel NVARCHAR(10),
    @p_EngineType NVARCHAR(10)
AS
BEGIN
    SELECT me.Id, me.MaterialCode, me.ClassType, me.ClassName, me.TransmissionType, 
           me.EngineModel, me.EngineType
      FROM MasterEngine me
     WHERE (@p_MaterialCode IS NULL OR me.MaterialCode LIKE CONCAT('%', @p_MaterialCode, '%'))
       AND (@p_TransmissionType IS NULL OR me.TransmissionType LIKE CONCAT('%', @p_TransmissionType, '%'))
       AND (@p_EngineModel IS NULL OR me.EngineModel LIKE CONCAT('%', @p_EngineModel, '%'))
       AND (@p_EngineType IS NULL OR me.EngineType LIKE CONCAT('%', @p_EngineType, '%'))
       AND me.IsDeleted = 0
END
------------------------------------------------Shipment------------------------------------------------
CREATE PROCEDURE INV_PROD_SHIPMENT_SEARCH
(
    @p_ShipmentNo NVARCHAR(255),
    @p_ShippingcompanyCode NVARCHAR(255),
    @p_SupplierNo NVARCHAR(255),
    @p_FromPort NVARCHAR(255),
    @p_ToPort NVARCHAR(255),
    @p_ShipmentDate NVARCHAR(255)
)
AS 
   SELECT DISTINCT a.Id, a.ShipmentNo, a.ShippingcompanyCode, a.SupplierNo,
          a.Buyer, a.FromPort, a.ToPort, a.ShipmentDate,
          a.Etd, a.Eta, a.Ata, a.OceanVesselName, a.Atd, a.Status
     FROM ProdShipment a
LEFT JOIN ProdBillOfLading b 
       ON a.Id = b.ShipmentId
LEFT JOIN ProdInvoice c 
       ON c.BillId = b.Id
    WHERE (@p_ShipmentNo IS NULL OR a.ShipmentNo LIKE CONCAT('%', @p_ShipmentNo, '%'))
      AND (@p_ShippingcompanyCode IS NULL OR a.ShippingcompanyCode LIKE CONCAT('%', @p_ShippingcompanyCode, '%'))
      AND (@p_SupplierNo IS NULL OR a.SupplierNo LIKE CONCAT('%', @p_SupplierNo, '%'))
      AND (@p_FromPort IS NULL OR a.FromPort LIKE CONCAT('%', @p_FromPort, '%'))
      AND (@p_ToPort IS NULL OR a.ToPort LIKE CONCAT('%', @p_ToPort, '%'))
      AND (@p_ShipmentDate IS NULL OR a.ShipmentDate LIKE CONCAT('%', @p_ShipmentDate, '%'))
      AND a.IsDeleted = 0
 ORDER BY a.Etd DESC, a.Eta DESC
------------------------------------------------BillOfLading------------------------------------------------
CREATE PROCEDURE INV_PROD_BILL_OF_LADING_SEARCH
(
    @p_BillofladingNo NVARCHAR(255),
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
    @p_InvoiceNo NVARCHAR(255),
    @p_InvoiceDateFrom DATE,
    @p_InvoiceDateTo DATE,
    @p_BillNo NVARCHAR(255),
    @p_ShipmentNo NVARCHAR(255),
    @p_ContainerNo NVARCHAR(255),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE,
    @p_OrderTypeCode NVARCHAR(255),
    @p_SupplierNo NVARCHAR(255)
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
	     AND (@p_OrderTypeCode IS NULL OR a.OrdertypeCode = @p_OrderTypeCode)
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
    @p_BillNo NVARCHAR(50),
    @p_ContainerNo NVARCHAR(255),
    @p_InvoiceNo NVARCHAR(255),
    @p_SealNo NVARCHAR(255),
    @p_Status NVARCHAR(255),
    @p_SupplierNo NVARCHAR(255),
    @p_BillDateFrom DATE,
    @p_BillDateTo DATE
)
AS 
    SELECT a.Id, a.ContainerNo, a.InvoiceId, a.SupplierNo, a.SealNo, a.ContainerSize, 
           a.PlanDevanningDate, a.ActualDevanningDate, a.Thc, b.Description Status, 
           a.ThcVn, a.Freight, a.Insurance, a.Tax, a.Amount, a.TaxVnd, a.VatVnd, 
           c.BillofladingNo, c.BillDate
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
  ORDER BY c.BillDate DESC, a.PlanDevanningDate DESC, b.Description

GO
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