-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- ============================================
CREATE PROCEDURE ObtenerVehiculo
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Vehiculo.Id AS Expr1, Vehiculo.IdModelo AS Expr2, Vehiculo.Placa AS Expr3, Vehiculo.Id, Vehiculo.IdModelo, Vehiculo.Placa, Vehiculo.Color, Vehiculo.Anio, Vehiculo.Precio, Vehiculo.CorreoPropietario, Vehiculo.TelefonoPropietario, Vehiculo.Color AS Expr4, Vehiculo.Anio AS Expr5, 
             Vehiculo.Precio AS Expr6, Vehiculo.CorreoPropietario AS Expr7, Vehiculo.TelefonoPropietario AS Expr8, Modelos.Nombre AS Modelo, Marcas.Nombre AS Marca
FROM   Vehiculo INNER JOIN
             Modelos ON Vehiculo.IdModelo = Modelos.Id INNER JOIN
             Marcas ON Modelos.IdMarca = Marcas.Id
WHERE (Vehiculo.Id = @Id)
END