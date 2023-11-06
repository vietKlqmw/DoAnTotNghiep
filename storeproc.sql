------------------------------------------------Material------------------------------------------------
--Search
CREATE PROCEDURE INV_MASTER_MATERIAL_SEARCH
    @p_MaterialCode NVARCHAR(40),
    @p_MaterialGroup NVARCHAR(9)
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.Plant, mm.StorageLocation, mm.ProductionGroup, 
           mm.ProductionPurpose, mm.ReservedStock, mm.LotCode, mm.ProductionStorageLocation, 
           mm.CostingLotSize, mm.ProductionVersion, mm.StandardPrice, mm.MovingPrice, 
           mm.MaterialOrigin, mm.OriginGroup, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE (@p_MaterialCode IS NULL OR mm.MaterialCode LIKE CONCAT('%', @p_MaterialCode, '%'))
       AND (@p_MaterialGroup IS NULL OR mm.MaterialGroup LIKE CONCAT('%', @p_MaterialGroup, '%'))
END
--Get data by Id:
CREATE PROCEDURE INV_MASTER_MATERIAL_BY_ID
    @p_MaterialId BIGINT
AS
BEGIN
    SELECT mm.Id, mm.MaterialType, mm.MaterialCode, mm.Description, mm.MaterialGroup, 
           mm.BaseUnitOfMeasure, mm.Plant, mm.StorageLocation, mm.ProductionGroup, 
           mm.ProductionPurpose, mm.ReservedStock, mm.LotCode, mm.ProductionStorageLocation, 
           mm.CostingLotSize, mm.ProductionVersion, mm.StandardPrice, mm.MovingPrice, 
           mm.MaterialOrigin, mm.OriginGroup, mm.EffectiveDateFrom, mm.EffectiveDateTo
      FROM MasterMaterial mm
     WHERE mm.Id = @p_MaterialId
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
END