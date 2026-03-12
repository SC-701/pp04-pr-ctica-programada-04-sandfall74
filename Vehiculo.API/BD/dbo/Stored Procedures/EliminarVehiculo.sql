CREATE PROCEDURE EliminarVehiculo
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	begin transaction
    -- Insert statements for procedure here
	DELETE
FROM   Vehiculo 
WHERE (Vehiculo.Id = @Id)
select @Id as 'Id'
	commit transaction
END