--Pedidos de Clientes
CREATE OR ALTER VIEW dbo.vw01PedidosReportes
AS
SELECT
  p.IdPedido,
  p.Fecha      AS FechaPedido,
  e.Nombre     AS EmpleadoNombre,
  sp.Status    AS Status,
  d.CodigoPostal,
  c.Telefono   AS TelefonoCliente,
  p.Mesa,
  p.Total
FROM dbo.Pedidoes p
LEFT JOIN dbo.Empleadoes       e  ON p.IdEmpleado       = e.IdEmpleado
LEFT JOIN dbo.StatusPedidoes    sp ON p.IdStatusPedido   = sp.IdStatusPedido
LEFT JOIN dbo.Direccions     d  ON p.IdDireccion      = d.IdDireccion
LEFT JOIN dbo.Clientes        c  ON p.IdCliente        = c.IdCliente;
GO

--Inventario
CREATE OR ALTER VIEW dbo.vw02InventarioReportes
AS
SELECT
  i.IdInsumo,
  i.Nombre,
  ci.CategoriaInsumoNombre AS Categoria,
  um.UnidadDeMedidaNombre AS Unidad,
  i.Cantidad
FROM dbo.Insumoes i
LEFT JOIN dbo.CategoriaInsumoes  ci ON i.IdCategoriaInsumo = ci.IdCategoriaInsumo
LEFT JOIN dbo.UnidadDeMedidas  um ON i.IdUnidadDeMedida  = um.IdUnidadDeMedida;
GO

--Pedidos a Proveedor
CREATE OR ALTER VIEW dbo.vw03PedidoProveedorReportes
AS
SELECT
  pp.IdPedidoProveedor,
  pp.FechaPedido,
  pr.Nombre        AS ProveedorNombre,
  pp.Total,
  pp.Status
FROM dbo.PedidoProveedors pp
LEFT JOIN dbo.Proveedors pr 
  ON pp.IdProveedor = pr.IdProveedor;
GO

--AQUI IRIA LA MERMA PERO NO HAY TABLA PIPIPI