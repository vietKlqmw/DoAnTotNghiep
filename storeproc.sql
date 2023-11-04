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