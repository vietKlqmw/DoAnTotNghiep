------------------------------------------------VehicleCBU------------------------------------------------
CREATE PROCEDURE INV_MASTER_VEHICLE_CBU_SEARCH
    @p_VehicleType NVARCHAR(10),
    @p_Model NVARCHAR(MAX)
AS
BEGIN
    SELECT mvc.Id, mvc.VehicleType, mvc.Model, mvc.MarketingCode, mvc.ProductionCode 
      FROM MasterVehicleCBU mvc
     WHERE @p_VehicleType IS NULL OR mvc.VehicleType LIKE CONCAT('%',@p_VehicleType,'%')
       AND @p_Model IS NULL OR mvc.Model LIKE CONCAT('%',@p_Model,'%')
END