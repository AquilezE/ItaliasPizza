--Pedidos de Clientes
CREATE OR ALTER PROCEDURE dbo.sp01_GetPedidosReportes
  @Desde DATE,
  @Hasta DATE
AS
BEGIN
  SET NOCOUNT ON;

  SELECT *
  FROM dbo.vw01PedidosReportes
  WHERE FechaPedido BETWEEN @Desde AND @Hasta
  ORDER BY FechaPedido;
END;
GO

--Inventario (sin filtro de fechas)
CREATE OR ALTER PROCEDURE dbo.sp02_GetInventarioReport
AS
BEGIN
  SET NOCOUNT ON;

  SELECT *
  FROM dbo.vw02InventarioReportes
  ORDER BY Nombre;
END;
GO

--Pedidos a Proveedor
CREATE OR ALTER PROCEDURE dbo.sp03_GetPedidosProveedorReport
  @Desde DATE,
  @Hasta DATE
AS
BEGIN
  SET NOCOUNT ON;

  SELECT *
  FROM dbo.vw03PedidoProveedorReportes
  WHERE FechaPedido BETWEEN @Desde AND @Hasta
  ORDER BY FechaPedido;
END;
GO

--NO HAY MERMA PIPIPIPI