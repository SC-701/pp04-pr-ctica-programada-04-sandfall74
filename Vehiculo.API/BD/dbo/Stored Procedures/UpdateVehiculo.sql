CREATE PROCEDURE UpdateVehiculo
	-- Add the parameters for the stored procedure here
	@Id AS uniqueidentifier
,@IdModelo AS uniqueidentifier
,@Placa AS varchar(max)
,@Color AS varchar(max)
,@Anio AS int
,@Precio AS decimal(18,0)
,@CorreoPropietario AS varchar(max)
,@TelefonoPropietario AS varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    begin transaction
    -- Insert statements for procedure here
	UPDATE [dbo].[Vehiculo]
   SET 
      [IdModelo] = @IdModelo
      ,[Placa] = @Placa
      ,[Color] = @Color
      ,[Anio] = @Anio
      ,[Precio] = @Precio
      ,[CorreoPropietario] = @CorreoPropietario
      ,[TelefonoPropietario] = @TelefonoPropietario
 WHERE Id = @Id
 select @Id as 'Id'
 commit transaction
END