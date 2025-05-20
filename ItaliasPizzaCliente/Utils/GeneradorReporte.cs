using ItaliasPizzaDB.DataAccessObjects;
using ItaliasPizzaDB.DataTransferObjects;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizzaCliente.Utils
{
    public class GeneradorReporte
    {

        public static string GenerarHTMLReporte(OpcionesReporte opcionesReporte)
        {
            var html = new StringBuilder();

            // … inside GenerateReportHtml, before any <h1> …
            html.AppendLine("<head>");
            html.AppendLine("<meta charset=\"utf-8\"/>");
            html.AppendLine("<style>");
            html.AppendLine("  body {");
            html.AppendLine("    font-family: 'Segoe UI', Tahoma, sans-serif;");
            html.AppendLine("    color: #333;");
            html.AppendLine("    margin: 20px;");
            html.AppendLine("  }");
            html.AppendLine("  h1 {");
            html.AppendLine("    font-size: 24px;");
            html.AppendLine("    margin-bottom: 0.5em;");
            html.AppendLine("  }");
            html.AppendLine("  h2 {");
            html.AppendLine("    font-size: 20px;");
            html.AppendLine("    margin-top: 1.5em;");
            html.AppendLine("    margin-bottom: 0.5em;");
            html.AppendLine("    border-bottom: 1px solid #ccc;");
            html.AppendLine("    padding-bottom: 0.2em;");
            html.AppendLine("  }");
            html.AppendLine("  table {");
            html.AppendLine("    width: 100%;");
            html.AppendLine("    border-collapse: collapse;");
            html.AppendLine("    margin-bottom: 1.5em;");
            html.AppendLine("  }");
            html.AppendLine("  th, td {");
            html.AppendLine("    border: 1px solid #ddd;");
            html.AppendLine("    padding: 8px;");
            html.AppendLine("    text-align: left;");
            html.AppendLine("  }");
            html.AppendLine("  th {");
            html.AppendLine("    background-color: #f9f9f9;");
            html.AppendLine("  }");
            html.AppendLine("  tr:nth-child(even) {");
            html.AppendLine("    background-color: #fbfbfb;");
            html.AppendLine("  }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendFormat("<h1>Reporte desde {0:yyyy-MM-dd} hasta {1:yyyy-MM-dd}</h1>",
                opcionesReporte.FechaInicio, opcionesReporte.FechaFin);


            if (opcionesReporte.IncluirPedidos)
            {
                var pedidos = PedidoDAO.ObtenerPedidosReporte(opcionesReporte.FechaInicio, opcionesReporte.FechaFin);
                if (pedidos.Any())
                {
                    html.Append(BuildPedidosSection(pedidos));
                    html.Append(BuildPedidosMetrics(pedidos));
                }
                else
                {
                    html.AppendLine("<h2>Pedidos de Clientes</h2>");
                    html.AppendLine("<p>No se encontraron registros</p>");
                }
            }

            if (opcionesReporte.IncluirInventario)
            {
                var inventario = InsumoDAO.ObtenerInventarioReporte();
                if (inventario.Any())
                {
                    html.Append(BuildInventarioSection(inventario));
                    html.Append(BuildInventarioMetrics(inventario));
                }
                else
                {
                    html.AppendLine("<h2>Inventario</h2>");
                    html.AppendLine("<p>No se encontraron registros</p>");
                }
            }

            if (opcionesReporte.IncluirPedidosProveedor)
            {
                var pedidosProv = PedidoProveedorDAO.ObtenerPedidosProveedorReporte(opcionesReporte.FechaInicio, opcionesReporte.FechaFin);
                if (pedidosProv.Any())
                {
                    html.Append(BuildPedidosProveedorSection(pedidosProv));
                    html.Append(BuildPedidosProveedorMetrics(pedidosProv));
                }
                else
                {
                    html.AppendLine("<h2>Pedidos a Proveedor</h2>");
                    html.AppendLine("<p>No se encontraron registros</p>");
                }
            }

            html.AppendLine("</body></html>");

            return html.ToString();
        }


        private static string BuildPedidosSection(IEnumerable<PedidoDTO> pedidos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Pedidos de Clientes</h2>");
            sb.AppendLine("<table><thead>"
                        + "<tr><th>Folio</th><th>Fecha</th><th>Empleado</th><th>Status</th>"
                        + "<th>C.P.</th><th>Teléfono</th><th>Mesa</th><th>Total</th></tr>"
                        + "</thead><tbody>");
            foreach (var p in pedidos)
            {
                if (p.CodigoPostal == null)
                    p.CodigoPostal = "N/A";
                if (p.TelefonoCliente == null)
                    p.TelefonoCliente = "N/A";
                if (p.Mesa == 0)
                    p.Mesa = -1;

                sb.AppendFormat("<tr><td>{0}</td><td>{1:yyyy-MM-dd}</td><td>{2}</td><td>{3}</td>"
                              + "<td>{4}</td><td>{5}</td><td>{6}</td><td>{7:C}</td></tr>",
                                p.IdPedido, p.FechaPedido, p.EmpleadoNombre,
                                p.Status, p.CodigoPostal, p.TelefonoCliente,
                                p.Mesa, p.Total);

            }
            sb.AppendLine("</tbody></table>");
            return sb.ToString();
        }

        private static string BuildInventarioSection(IEnumerable<InsumoDTO> insumos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Inventario</h2>");
            sb.AppendLine("<table><thead>"
                        + "<tr><th>ID</th><th>Nombre</th><th>Categoría</th><th>Unidad</th><th>Cantidad</th></tr>"
                        + "</thead><tbody>");
            foreach (var i in insumos)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                                i.IdInsumo, i.Nombre, i.Categoria, i.Unidad, i.Cantidad);
            }
            sb.AppendLine("</tbody></table>");
            return sb.ToString();
        }

        private static string BuildPedidosProveedorSection(IEnumerable<PedidoProveedorDTO> pp)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>Pedidos a Proveedor</h2>");
            sb.AppendLine("<table><thead>"
                        + "<tr><th>Folio</th><th>Fecha</th><th>Proveedor</th>"
                        + "<th>Total</th><th>Status</th></tr>"
                        + "</thead><tbody>");
            foreach (var r in pp)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1:yyyy-MM-dd}</td><td>{2}</td>"
                              + "<td>{3:C}</td><td>{4}</td></tr>",
                                r.IdPedidoProveedor, r.FechaPedido,
                                r.ProveedorNombre, r.Total, r.Status);
            }
            sb.AppendLine("</tbody></table>");
            return sb.ToString();
        }

        private static string BuildPedidosMetrics(IEnumerable<PedidoDTO> pedidos)
        {
            var lista = pedidos.ToList();
            int totalCount = lista.Count;
            decimal ingreso = lista.Sum(p => p.Total);
            decimal promedio = totalCount > 0 ? ingreso / totalCount : 0m;

            var sb = new StringBuilder();
            sb.AppendLine("<h2>Métricas de Pedidos</h2>");
            sb.AppendLine("<ul class=\"metrics\">");
            sb.AppendFormat("<li>Total de pedidos: {0}</li>", totalCount);
            sb.AppendFormat("<li>Ingreso total: {0:C}</li>", ingreso);
            sb.AppendFormat("<li>Promedio por pedido: {0:C}</li>", promedio);
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static string BuildInventarioMetrics(IEnumerable<InsumoDTO> insumos)
        {
            var lista = insumos.ToList();
            int distinct = lista.Count;
            float suma = lista.Sum(i => i.Cantidad);

            // Top 3 categorías por cantidad
            var topCats = lista
                .GroupBy(i => i.Categoria)
                .Select(g => new { Categoria = g.Key, Total = g.Sum(i => i.Cantidad) })
                .OrderByDescending(x => x.Total)
                .Take(3)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("<h2>Métricas de Inventario</h2>");
            sb.AppendLine("<ul class=\"metrics\">");
            sb.AppendFormat("<li>Número de insumos distintos: {0}</li>", distinct);
            sb.AppendFormat("<li>Cantidad total en inventario: {0}</li>", suma);
            sb.AppendLine("<li>Categorías con mayor cantidad:</li>");
            sb.AppendLine("<ul>");
            foreach (var c in topCats)
                sb.AppendFormat("<li>{0}: {1}</li>", c.Categoria, c.Total);
            sb.AppendLine("</ul>");
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static string BuildPedidosProveedorMetrics(IEnumerable<PedidoProveedorDTO> pp)
        {
            var lista = pp.ToList();
            int totalCount = lista.Count;
            decimal gasto = lista.Sum(x => x.Total);
            decimal promedio = totalCount > 0 ? gasto / totalCount : 0m;

            // Top 3 proveedores por gasto
            var topProvs = lista
                .GroupBy(x => x.ProveedorNombre)
                .Select(g => new { Proveedor = g.Key, Total = g.Sum(i => i.Total) })
                .OrderByDescending(x => x.Total)
                .Take(3)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("<h2>Métricas de Pedidos a Proveedor</h2>");
            sb.AppendLine("<ul class=\"metrics\">");
            sb.AppendFormat("<li>Total de órdenes: {0}</li>", totalCount);
            sb.AppendFormat("<li>Gasto total en compras: {0:C}</li>", gasto);
            sb.AppendFormat("<li>Promedio por orden: {0:C}</li>", promedio);
            sb.AppendLine("<li>Principales proveedores por gasto:</li>");
            sb.AppendLine("<ul>");
            foreach (var p in topProvs)
                sb.AppendFormat("<li>{0}: {1:C}</li>", p.Proveedor, p.Total);
            sb.AppendLine("</ul>");
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static byte[] ConvertHtmlToPdf(string htmlContent)
        {
            using (MemoryStream pdfStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(pdfStream);
                PdfDocument pdfDoc = new PdfDocument(writer);

                ConverterProperties converterProperties = new ConverterProperties();

                HtmlConverter.ConvertToPdf(htmlContent, pdfDoc, converterProperties);

                pdfDoc.Close();

                return pdfStream.ToArray();
            }
        }

    }

    public class OpcionesReporte
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool IncluirPedidos { get; set; }
        public bool IncluirInventario { get; set; }
        public bool IncluirMerma { get; set; } = false;
        public bool IncluirPedidosProveedor { get; set; }
    }
}
