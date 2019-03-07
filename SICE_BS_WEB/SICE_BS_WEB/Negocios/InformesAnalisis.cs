using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;

namespace SICE_BS_WEB.Negocios
{
    public class InformesAnalisis
    {
        //Singleton singleton = Singleton.Global;
        ControlExcepciones excepcion = new ControlExcepciones();


        public DataTable ConsultarDIOT(DateTime? DESDE, DateTime? HASTA, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Diot");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarReporteAnual(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ReporteAnual");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = PATENTES == null ? Convert.DBNull : PATENTES;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarDTA(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("DTA");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = PATENTES == null ? Convert.DBNull : PATENTES;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarCompulsa(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string PATENTES, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("Compulsa");
            SqlConnection v_con = ConectarBD.getConexion(cadena);

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@PATENTES", SqlDbType.VarChar).Value = PATENTES == null ? Convert.DBNull : PATENTES;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarCG(string PEDIMENTOARMADO, DateTime? DESDE, DateTime? HASTA, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CG");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }


        #region Hoja de Cálculo

        public DataTable ConsultarHojaCalculo(string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ConsultaHC");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarPorPedimentoEnHojaCalculo(string PEDIMENTOARMADO, string cadena, ref string mensaje, out string v_mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("CreaHC");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        //cmd.Parameters.Add("@HCKEY", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                        v_mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                        //v_hckey = cmd.Parameters["@HCKEY"].Value.ToString();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);

                v_mensaje = ex.Message;
            }
            return dt;
        }

        public DataTable ConsultarPorFechasEnHojaCalculo(DateTime? DESDE, DateTime? HASTA, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "creaHojaDeCalculo_periodo";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarDatosEdicion(string v_hckey, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ConsultaHC_HCKEY");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Original_hckey", SqlDbType.UniqueIdentifier).Value = new Guid(v_hckey);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarDatosEdicionFactura(string v_hckey, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ConsultaHC_HCKEY_FACTURA");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@key", SqlDbType.UniqueIdentifier).Value = new Guid(v_hckey);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ValidaRegistrosEnDetalle(string v_hckey, string cadena, ref string mensaje, out string v_mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ValidaHCDF");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Original_hckey", SqlDbType.UniqueIdentifier).Value = new Guid(v_hckey);
                        cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                        v_mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
                v_mensaje = ex.Message;
            }
            return dt;
        }

        public DataTable ActualizaHojaCalculoYDetalle(string v_hckey, string _01_nombreimportador, string _01_rfcimportador, string _01_importadorcalle,
            string _01_importadornumero, string _01_importadorcodigop, string _01_importadorcolonia, string _02_nombrevendedor, string _02_rfcvendedor, 
            string _02_vendedorcalle, string _02_vendedornumero, string _02_vendedorciudad, string _02_vendedorpais, string detpago1, string detpago2,
            string detpago3, string detpago4, string detpago5, string txt5pago, decimal se5PagoDirecto, decimal se5_Contraprestacion, decimal lbl05Total,
            string txt6_Moneda, decimal se6_Comision, decimal se6_FleteSeguro, decimal se6_CargaDescarga, decimal se6_MaterialAportado, decimal se6_TecnoAportada, 
            decimal se6_Regalias, decimal se6_Reversiones, decimal lbl06Total,  string txt7_Moneda,decimal se7_GastoNoRelacionado,decimal se7_FleteSeguro,
            decimal se7_GastosConstruccion, decimal se7_Inst,decimal se7_Contribuciones,decimal se7_Dividendos,decimal lbl07Total,decimal se8_Pagar,string txt8_Moneda1,
            decimal se8_Ajuste,string txt8_Moneda2,decimal se8_Aduana,string txt8_Moneda3,string txt9_PediNum, DateTime? date9_Fechapedimento,string txt9_FacturaNumero,
            DateTime? date9_FechaFactura, string v_chk9_MasPedimento,string txt9_LugarFactura,string v_chk9_FacturaUnica,string v_chk9_Subdivion,decimal se_ValorAduana,
            string v_chk12_Identicas,string v_chk12_Similares,string v_chk12_Venta,string v_chk12_Reconstruido,string v_chk12_Articulo,decimal valoraduana,
            Int64 detallehckey, string fraccion, string descripcion, decimal cantidad, string paisproduccion, string paisprocedencia, string nombre_ri,
            string rfc_ri, string patente, string referencia, int opcion, int repkey, int muestrafirma, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ActualizaHC");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Original_hckey", SqlDbType.UniqueIdentifier).Value = new Guid(v_hckey);
                        cmd.Parameters.Add("@01_nombreimportador", SqlDbType.VarChar).Value = _01_nombreimportador;
                        cmd.Parameters.Add("@01_rfcimportador", SqlDbType.VarChar).Value = _01_rfcimportador;
                        cmd.Parameters.Add("@01_importadorcalle", SqlDbType.VarChar).Value = _01_importadorcalle;
                        cmd.Parameters.Add("@01_importadornumero", SqlDbType.VarChar).Value = _01_importadornumero;
                        cmd.Parameters.Add("@01_importadorcodigop", SqlDbType.VarChar).Value = _01_importadorcodigop;
                        cmd.Parameters.Add("@01_importadorcolonia", SqlDbType.VarChar).Value = _01_importadorcolonia;
                        cmd.Parameters.Add("@02_nombrevendedor", SqlDbType.VarChar).Value = _02_nombrevendedor;
                        cmd.Parameters.Add("@02_rfcvendedor", SqlDbType.VarChar).Value = _02_rfcvendedor;
                        cmd.Parameters.Add("@02_vendedorcalle", SqlDbType.VarChar).Value = _02_vendedorcalle;
                        cmd.Parameters.Add("@02_vendedornumero", SqlDbType.VarChar).Value = _02_vendedornumero;
                        cmd.Parameters.Add("@02_vendedorciudad", SqlDbType.VarChar).Value = _02_vendedorciudad;
                        cmd.Parameters.Add("@02_vendedorpais", SqlDbType.VarChar).Value = _02_vendedorpais;
                        cmd.Parameters.Add("@04_determinacionpago_01", SqlDbType.VarChar).Value = detpago1;
                        cmd.Parameters.Add("@04_determinacionpago_02", SqlDbType.VarChar).Value = detpago2;
                        cmd.Parameters.Add("@04_determinacionpago_03", SqlDbType.VarChar).Value = detpago3;
                        cmd.Parameters.Add("@04_determinacionpago_04", SqlDbType.VarChar).Value = detpago4;
                        cmd.Parameters.Add("@04_determinacionpago_05", SqlDbType.VarChar).Value = detpago5;
                        cmd.Parameters.Add("@05_pagodirecto", SqlDbType.Decimal).Value = se5PagoDirecto;
                        cmd.Parameters.Add("@05_contraprestacionpagodirecto", SqlDbType.Decimal).Value = se5_Contraprestacion;
                        cmd.Parameters.Add("@05_totalpreciopagado", SqlDbType.Decimal).Value = lbl05Total;
                        cmd.Parameters.Add("@05_monedapagodirecto", SqlDbType.VarChar).Value = txt5pago;
                        cmd.Parameters.Add("@06_incrementablescomision", SqlDbType.Decimal).Value = se6_Comision;
                        cmd.Parameters.Add("@06_incremetablesfletes", SqlDbType.Decimal).Value = se6_FleteSeguro;
                        cmd.Parameters.Add("@06_incremetablesfletescarga", SqlDbType.Decimal).Value = se6_CargaDescarga;
                        cmd.Parameters.Add("@06_incremetablesmateriales", SqlDbType.Decimal).Value = se6_MaterialAportado;
                        cmd.Parameters.Add("@06_incremetablestecnologias", SqlDbType.Decimal).Value = se6_TecnoAportada;
                        cmd.Parameters.Add("@06_incremetablesregalias", SqlDbType.Decimal).Value = se6_Regalias;
                        cmd.Parameters.Add("@06_incremetablesreversiones", SqlDbType.Decimal).Value = se6_Reversiones;
                        cmd.Parameters.Add("@06_incremetablestotal", SqlDbType.Decimal).Value = lbl06Total;
                        cmd.Parameters.Add("@06_incrementablemoneda", SqlDbType.VarChar).Value = txt6_Moneda;
                        cmd.Parameters.Add("@07_noincremetablesrelacionados",SqlDbType.Decimal).Value = se7_GastoNoRelacionado;
                        cmd.Parameters.Add("@07_noincremetablesfletes", SqlDbType.Decimal).Value = se7_FleteSeguro;
                        cmd.Parameters.Add("@07_noincremetablescontribucion", SqlDbType.Decimal).Value = se7_Contribuciones;
                        cmd.Parameters.Add("@07_noincremetablesotros", SqlDbType.Decimal).Value = se7_Inst;
                        cmd.Parameters.Add("@07_noincremetablescontribuciones", SqlDbType.Decimal).Value = se7_Contribuciones;
                        cmd.Parameters.Add("@07_noincremetablesdividendos", SqlDbType.Decimal).Value = se7_Dividendos;
                        cmd.Parameters.Add("@07_noincremetablestotal", SqlDbType.Decimal).Value = lbl07Total;
                        cmd.Parameters.Add("@07_noincrementablesmodena", SqlDbType.VarChar).Value = txt7_Moneda;
                        cmd.Parameters.Add("@08_preciopagadoporpagar", SqlDbType.Decimal).Value = se8_Pagar;
                        cmd.Parameters.Add("@08_monedapreciopagadoporpagar", SqlDbType.VarChar).Value = txt8_Moneda1;
                        cmd.Parameters.Add("@08_ajusteincrementables", SqlDbType.Decimal).Value = se8_Ajuste;
                        cmd.Parameters.Add("@08_monedaajusteincrementables", SqlDbType.VarChar).Value = txt8_Moneda2;
                        cmd.Parameters.Add("@08_valorenaduana", SqlDbType.Decimal).Value = se8_Aduana;
                        cmd.Parameters.Add("@08_monedavalorenaduana", SqlDbType.VarChar).Value = txt8_Moneda3;
                        cmd.Parameters.Add("@09_pedimentonumero", SqlDbType.VarChar).Value = txt9_PediNum;
                        cmd.Parameters.Add("@09_fechapedimento", SqlDbType.DateTime).Value = date9_Fechapedimento == null ? Convert.DBNull : date9_Fechapedimento;
                        cmd.Parameters.Add("@09_facturanumero", SqlDbType.VarChar).Value = txt9_FacturaNumero;
                        cmd.Parameters.Add("@09_fechafactura", SqlDbType.DateTime).Value = date9_FechaFactura == null ? Convert.DBNull : date9_FechaFactura;
                        cmd.Parameters.Add("@09_masdeunpedimento", SqlDbType.VarChar).Value = v_chk9_MasPedimento;
                        cmd.Parameters.Add("@09_lugaremisionfactura", SqlDbType.VarChar).Value = txt9_LugarFactura;
                        cmd.Parameters.Add("@09_tipofacturaunico", SqlDbType.VarChar).Value = v_chk9_FacturaUnica;
                        cmd.Parameters.Add("@09_subdivision", SqlDbType.VarChar).Value = v_chk9_Subdivion;
                        cmd.Parameters.Add("@10_valoraduanadeterminado", SqlDbType.Decimal).Value = valoraduana;
                        cmd.Parameters.Add("@12_valordetransaccionesidenticas", SqlDbType.VarChar).Value = v_chk12_Identicas;
                        cmd.Parameters.Add("@12_valordetransaccionessimilares", SqlDbType.VarChar).Value = v_chk12_Similares;
                        cmd.Parameters.Add("@12_valorpreciounitario", SqlDbType.VarChar).Value = v_chk12_Venta;
                        cmd.Parameters.Add("@12_valorreconstruido", SqlDbType.VarChar).Value = v_chk12_Reconstruido;
                        cmd.Parameters.Add("@12_valordeterminado", SqlDbType.VarChar).Value = v_chk12_Articulo;
                        cmd.Parameters.Add("@detallehckey", SqlDbType.BigInt).Value = detallehckey;
                        cmd.Parameters.Add("@fraccion", SqlDbType.VarChar).Value = fraccion;
                        cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;
                        cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = cantidad;
                        cmd.Parameters.Add("@paisproduccion", SqlDbType.VarChar).Value = paisproduccion;
                        cmd.Parameters.Add("@paisprocedencia", SqlDbType.VarChar).Value = paisprocedencia;
                        cmd.Parameters.Add("@nombrerepresentntelegal", SqlDbType.VarChar).Value = nombre_ri;
                        cmd.Parameters.Add("@rfcrepresentntelegal", SqlDbType.VarChar).Value = rfc_ri;
                        cmd.Parameters.Add("@patente", SqlDbType.VarChar).Value = patente;
                        cmd.Parameters.Add("@opcion", SqlDbType.VarChar).Value = opcion;
                        cmd.Parameters.Add("@referencia", SqlDbType.VarChar).Value = referencia;
                        cmd.Parameters.Add("@REPRESENTANTEKEY", SqlDbType.VarChar).Value = repkey == 0 ? Convert.DBNull : repkey;
                        cmd.Parameters.Add("@MuestraFirma", SqlDbType.Bit).Value = muestrafirma;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (DBConcurrencyException ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ActualizaFacturas(Int64 v_facturakey, string factura, string lugaremision, string subdivision, string proveedor, string taxnumero, 
                                           string calle, string intext, string ciudad, string pais, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = NombreSP("ActualizaHCFactura");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Factura", SqlDbType.VarChar).Value = factura == string.Empty ? Convert.DBNull : factura;
                        cmd.Parameters.Add("@LugarEmision", SqlDbType.VarChar).Value = lugaremision == string.Empty ? Convert.DBNull : lugaremision;
                        cmd.Parameters.Add("@Subdivision", SqlDbType.VarChar).Value = subdivision == string.Empty ? Convert.DBNull : subdivision;
                        cmd.Parameters.Add("@Proveedor", SqlDbType.VarChar).Value = proveedor == string.Empty ? Convert.DBNull : proveedor;
                        cmd.Parameters.Add("@TaxNumero", SqlDbType.VarChar).Value = taxnumero == string.Empty ? Convert.DBNull : taxnumero;
                        cmd.Parameters.Add("@Calle", SqlDbType.VarChar).Value = calle == string.Empty ? Convert.DBNull : calle;
                        cmd.Parameters.Add("@Numero", SqlDbType.VarChar).Value = intext == string.Empty ? Convert.DBNull : intext;
                        cmd.Parameters.Add("@Ciudad", SqlDbType.VarChar).Value = ciudad == string.Empty ? Convert.DBNull : ciudad;
                        cmd.Parameters.Add("@Pais", SqlDbType.VarChar).Value = pais == string.Empty ? Convert.DBNull : pais;
                        cmd.Parameters.Add("@FacturaKey", SqlDbType.BigInt).Value = v_facturakey;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (DBConcurrencyException ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable EliminaRegistroEnHojaCalculo(string v_hckey, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();



            string sp = NombreSP("EliminaHC");

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Original_hckey", SqlDbType.UniqueIdentifier).Value = new Guid(v_hckey);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        #endregion



        #region Manifestaciónde Valor

        public DataTable ConsultarPorPedimentoEnMV(string PEDIMENTOARMADO, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "creaManifestacionDeValor";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarPorFechasEnMV(DateTime? DESDE, DateTime? HASTA, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "creaManifestacionDeValor_periodo";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DESDE", SqlDbType.DateTime).Value = DESDE == null ? Convert.DBNull : DESDE;
                        cmd.Parameters.Add("@HASTA", SqlDbType.DateTime).Value = HASTA == null ? Convert.DBNull : HASTA;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable ConsultarMV(string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "MV_SELECT";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable TraerMVParaEditar(string PEDIMENTOARMADO, Int64 v_key, int opcion, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();
            string sp = "MV_PEDIMENTO_KEY_SELECT";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = PEDIMENTOARMADO == null ? Convert.DBNull : PEDIMENTOARMADO;
                        cmd.Parameters.Add("@mvkeymanifestacion", SqlDbType.BigInt).Value = v_key == 0 ? Convert.DBNull : v_key;
                        cmd.Parameters.Add("@v_opcion", SqlDbType.Int).Value = opcion;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable EliminaMV(string v_mvkeymanifestacion, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();

            string sp = "MV_DELETE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mvkeymanifestacion", SqlDbType.BigInt).Value = Int64.Parse(v_mvkeymanifestacion);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        public DataTable Actualiza_ManifestacionDeValor(Int64 v_mvkeymanifestacion, string referencia, string a_nombre, string a_calle, string a_numeroexterior, 
            string a_numerointerior, string a_ciudad, string a_codigopostal, string a_estado, string a_pais, string a_telefono, string a_correo, string b_SIexistevinculacionimportadorvendedor,
            string b_NOexistevinculacionimportadorvendedor, string b_SIinfluyovalordetransaccion, string b_NOinfluyovalordetransaccion, string c_nombreimportador,
            string c_apellidopaternoimportador, string c_apellidomaternoimportador, string c_rfcimportador, string c_numeroexteriorimportador, string c_numerointeriorimportador,
            string c_ciudadimportador, string c_codigopostalimportador, string c_estadoimportador, string c_paisimportador, string c_telefonoimportador, string c_correoimportador,
            string d_apellidopaternoagente, string d_apellidomaternoagente, string d_nombreagente, string d_patenteagente, string f_unmetododevaloracion, string f_masdeunmetododevaloracion,
            string f_valordetransaccionmercancias, string f_descripciondetransaccionmercancias, string f_valordetransaccionmercanciasidenticas, string f_descripciondetransaccionmercanciasidenticas,
            string f_valordetransaccionmercanciassimilares, string f_descripciondetransaccionmercanciassimilares, string f_valordepreciounitariodeventa, string f_descripcionvalordepreciounitariodeventa,
            string f_valorreconstruido, string f_descripcionvalorreconstruido, string f_valorconformaley, string f_descripcionvalorconformaley, string g_anexadocumentacion,
            string g_anexadocumentacionnumeroyletra, string g_anexadocumentacionmodena, decimal ga_preciofacturanumero, string ga_preciofacturaletra, string gb_precioprevistofactura, string gb_otrosdocumentosanexados,
            string gb_conceptosenlaley, string gb_conceptosenlaleydesglozadosenfactura, string gd_NOanexadocumentacion, string gd_SIimportedeacuerdoalaley, string gd_NOimportedeacuerdoalaley,
            string gd_SIotrosdocumentosimportedeacuerdoalaley, string gd_NOotrosdocumentosimportedeacuerdoalaley, string gda_anexadocumentacion, string gd65_NOanexadocumentacion,
            string gda_SIderivadecompraventa, string gda_NOderivadecompraventa, string gda_SIanexadocumentovaloraduana, string gda_NOanexadocumentovaloraduana, string gdb_SIvalordeterminadopormercaciaprovicional,
            string gdb_NOvalordeterminadopormercaciaprovicional, string gdb_SIdocumentacionvalormercancia, string gdb_NOdocumentacionimportaciontemporal, string gdb_poroperacionimportaciontemporal,
            string gdb_poroperiodoimportaciontemporal, string gdb_rfcmanifestacion, DateTime? MV_FECHAPEDIMENTO, string gdb_nombremanifestacion, int repkey, int muestrafirma, string cadena, ref string mensaje)
        {
            mensaje = "";
            DataTable dt = new DataTable();

            string sp = "MV_UPDATE";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@mvkeymanifestacion", SqlDbType.BigInt).Value = v_mvkeymanifestacion;
                        cmd.Parameters.Add("@referencia", SqlDbType.VarChar).Value = referencia;
                        cmd.Parameters.Add("@a_nombre", SqlDbType.VarChar).Value = a_nombre;                        
                        cmd.Parameters.Add("@a_calle", SqlDbType.VarChar).Value = a_calle;
                        cmd.Parameters.Add("@a_numeroexterior", SqlDbType.VarChar).Value = a_numeroexterior;
                        cmd.Parameters.Add("@a_numerointerior", SqlDbType.VarChar).Value = a_numerointerior;
                        cmd.Parameters.Add("@a_ciudad", SqlDbType.VarChar).Value = a_ciudad;
                        cmd.Parameters.Add("@a_codigopostal", SqlDbType.VarChar).Value = a_codigopostal;
                        cmd.Parameters.Add("@a_estado", SqlDbType.VarChar).Value = a_estado;
                        cmd.Parameters.Add("@a_pais", SqlDbType.VarChar).Value = a_pais;
                        cmd.Parameters.Add("@a_telefono", SqlDbType.VarChar).Value = a_telefono;
                        cmd.Parameters.Add("@a_correo", SqlDbType.VarChar).Value = a_correo;
                        cmd.Parameters.Add("@b_SIexistevinculacionimportadorvendedor", SqlDbType.VarChar).Value = b_SIexistevinculacionimportadorvendedor;
                        cmd.Parameters.Add("@b_NOexistevinculacionimportadorvendedor", SqlDbType.VarChar).Value = b_NOexistevinculacionimportadorvendedor;
                        cmd.Parameters.Add("@b_SIinfluyovalordetransaccion", SqlDbType.VarChar).Value = b_SIinfluyovalordetransaccion;
                        cmd.Parameters.Add("@b_NOinfluyovalordetransaccion", SqlDbType.VarChar).Value = b_NOinfluyovalordetransaccion;
                        cmd.Parameters.Add("@c_nombreimportador", SqlDbType.VarChar).Value = c_nombreimportador;
                        cmd.Parameters.Add("@c_apellidopaternoimportador", SqlDbType.VarChar).Value = c_apellidopaternoimportador;
                        cmd.Parameters.Add("@c_apellidomaternoimportador", SqlDbType.VarChar).Value = c_apellidomaternoimportador;
                        cmd.Parameters.Add("@c_rfcimportador", SqlDbType.VarChar).Value = c_rfcimportador;
                        cmd.Parameters.Add("@c_numeroexteriorimportador", SqlDbType.VarChar).Value = c_numeroexteriorimportador;
                        cmd.Parameters.Add("@c_numerointeriorimportador", SqlDbType.VarChar).Value = c_numerointeriorimportador;
                        cmd.Parameters.Add("@c_ciudadimportador", SqlDbType.VarChar).Value = c_ciudadimportador;
                        cmd.Parameters.Add("@c_codigopostalimportador", SqlDbType.VarChar).Value = c_codigopostalimportador;
                        cmd.Parameters.Add("@c_estadoimportador", SqlDbType.VarChar).Value = c_estadoimportador;
                        cmd.Parameters.Add("@c_paisimportador", SqlDbType.VarChar).Value = c_paisimportador;
                        cmd.Parameters.Add("@c_telefonoimportador", SqlDbType.VarChar).Value = c_telefonoimportador;
                        cmd.Parameters.Add("@c_correoimportador", SqlDbType.VarChar).Value = c_correoimportador;
                        cmd.Parameters.Add("@d_apellidopaternoagente", SqlDbType.VarChar).Value = d_apellidopaternoagente;
                        cmd.Parameters.Add("@d_apellidomaternoagente", SqlDbType.VarChar).Value = d_apellidomaternoagente;
                        cmd.Parameters.Add("@d_nombreagente", SqlDbType.VarChar).Value = d_nombreagente;
                        cmd.Parameters.Add("@d_patenteagente", SqlDbType.VarChar).Value = d_patenteagente;
                        cmd.Parameters.Add("@f_unmetododevaloracion", SqlDbType.VarChar).Value = f_unmetododevaloracion;
                        cmd.Parameters.Add("@f_masdeunmetododevaloracion", SqlDbType.VarChar).Value = f_masdeunmetododevaloracion;
                        cmd.Parameters.Add("@f_valordetransaccionmercancias", SqlDbType.VarChar).Value = f_valordetransaccionmercancias;
                        cmd.Parameters.Add("@f_descripciondetransaccionmercancias", SqlDbType.VarChar).Value = f_descripciondetransaccionmercancias;
                        cmd.Parameters.Add("@f_valordetransaccionmercanciasidenticas", SqlDbType.VarChar).Value = f_valordetransaccionmercanciasidenticas;
                        cmd.Parameters.Add("@f_descripciondetransaccionmercanciasidenticas", SqlDbType.VarChar).Value = f_descripciondetransaccionmercanciasidenticas;
                        cmd.Parameters.Add("@f_valordetransaccionmercanciassimilares", SqlDbType.VarChar).Value = f_valordetransaccionmercanciassimilares;
                        cmd.Parameters.Add("@f_descripciondetransaccionmercanciassimilares", SqlDbType.VarChar).Value = f_descripciondetransaccionmercanciassimilares;
                        cmd.Parameters.Add("@f_valordepreciounitariodeventa", SqlDbType.VarChar).Value = f_valordepreciounitariodeventa;
                        cmd.Parameters.Add("@f_descripcionvalordepreciounitariodeventa", SqlDbType.VarChar).Value = f_descripcionvalordepreciounitariodeventa;
                        cmd.Parameters.Add("@f_valorreconstruido", SqlDbType.VarChar).Value = f_valorreconstruido;
                        cmd.Parameters.Add("@f_descripcionvalorreconstruido", SqlDbType.VarChar).Value = f_descripcionvalorreconstruido;
                        cmd.Parameters.Add("@f_valorconformaley", SqlDbType.VarChar).Value = f_valorconformaley;
                        cmd.Parameters.Add("@f_descripcionvalorconformaley", SqlDbType.VarChar).Value = f_descripcionvalorconformaley;
                        cmd.Parameters.Add("@g_anexadocumentacion", SqlDbType.VarChar).Value = g_anexadocumentacion;
                        cmd.Parameters.Add("@g_anexadocumentacionnumeroyletra", SqlDbType.VarChar).Value = g_anexadocumentacionnumeroyletra;
                        cmd.Parameters.Add("@g_anexadocumentacionmodena", SqlDbType.VarChar).Value = g_anexadocumentacionmodena;
                        cmd.Parameters.Add("@ga_preciofacturanumero", SqlDbType.Decimal).Value = ga_preciofacturanumero;
                        cmd.Parameters.Add("@ga_preciofacturaletra", SqlDbType.VarChar).Value = ga_preciofacturaletra;
                        cmd.Parameters.Add("@gb_precioprevistofactura", SqlDbType.VarChar).Value = gb_precioprevistofactura;
                        cmd.Parameters.Add("@gb_otrosdocumentosanexados", SqlDbType.VarChar).Value = gb_otrosdocumentosanexados;
                        cmd.Parameters.Add("@gb_conceptosenlaley", SqlDbType.VarChar).Value = gb_conceptosenlaley;
                        cmd.Parameters.Add("@gb_conceptosenlaleydesglozadosenfactura", SqlDbType.VarChar).Value = gb_conceptosenlaleydesglozadosenfactura;
                        cmd.Parameters.Add("@gd_NOanexadocumentacion", SqlDbType.VarChar).Value = gd_NOanexadocumentacion;
                        cmd.Parameters.Add("@gd_SIimportedeacuerdoalaley", SqlDbType.VarChar).Value = gd_SIimportedeacuerdoalaley;
                        cmd.Parameters.Add("@gd_NOimportedeacuerdoalaley", SqlDbType.VarChar).Value = gd_NOimportedeacuerdoalaley;
                        cmd.Parameters.Add("@gd_SIotrosdocumentosimportedeacuerdoalaley", SqlDbType.VarChar).Value = gd_SIotrosdocumentosimportedeacuerdoalaley;
                        cmd.Parameters.Add("@gd_NOotrosdocumentosimportedeacuerdoalaley", SqlDbType.VarChar).Value = gd_NOotrosdocumentosimportedeacuerdoalaley;
                        cmd.Parameters.Add("@gda_anexadocumentacion", SqlDbType.VarChar).Value = gda_anexadocumentacion;
                        cmd.Parameters.Add("@gd65_NOanexadocumentacion", SqlDbType.VarChar).Value = gd65_NOanexadocumentacion;
                        cmd.Parameters.Add("@gda_SIderivadecompraventa", SqlDbType.VarChar).Value = gda_SIderivadecompraventa;
                        cmd.Parameters.Add("@gda_NOderivadecompraventa", SqlDbType.VarChar).Value = gda_NOderivadecompraventa;
                        cmd.Parameters.Add("@gda_SIanexadocumentovaloraduana", SqlDbType.VarChar).Value = gda_SIanexadocumentovaloraduana;
                        cmd.Parameters.Add("@gda_NOanexadocumentovaloraduana", SqlDbType.VarChar).Value = gda_NOanexadocumentovaloraduana;
                        cmd.Parameters.Add("@gdb_SIvalordeterminadopormercaciaprovicional", SqlDbType.VarChar).Value = gdb_SIvalordeterminadopormercaciaprovicional;                                              
                        cmd.Parameters.Add("@gdb_NOvalordeterminadopormercaciaprovicional", SqlDbType.VarChar).Value = gdb_NOvalordeterminadopormercaciaprovicional;
                        cmd.Parameters.Add("@gdb_SIdocumentacionvalormercancia", SqlDbType.VarChar).Value = gdb_SIdocumentacionvalormercancia;
                        cmd.Parameters.Add("@gdb_NOdocumentacionimportaciontemporal", SqlDbType.VarChar).Value = gdb_NOdocumentacionimportaciontemporal;
                        cmd.Parameters.Add("@gdb_poroperacionimportaciontemporal", SqlDbType.VarChar).Value = gdb_poroperacionimportaciontemporal;
                        cmd.Parameters.Add("@gdb_poroperiodoimportaciontemporal", SqlDbType.VarChar).Value = gdb_poroperiodoimportaciontemporal;
                        cmd.Parameters.Add("@gdb_rfcmanifestacion", SqlDbType.VarChar).Value = gdb_rfcmanifestacion;
                        cmd.Parameters.Add("@MV_FECHAPEDIMENTO", SqlDbType.DateTime).Value = MV_FECHAPEDIMENTO == null ? Convert.DBNull : MV_FECHAPEDIMENTO;
                        cmd.Parameters.Add("@gdb_nombremanifestacion", SqlDbType.VarChar).Value = gdb_nombremanifestacion;
                        cmd.Parameters.Add("@REPRESENTANTEKEY", SqlDbType.VarChar).Value = repkey == 0 ? Convert.DBNull : repkey;
                        cmd.Parameters.Add("@MuestraFirma", SqlDbType.Bit).Value = muestrafirma;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                dt = null;
                            else
                                dt.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                excepcion.RegistrarExcepcion(0, sp, ex, cadena, ref mensaje);
                if (mensaje.Length == 0)
                    mensaje = "Error: " + excepcion.SerializarExMessage(cadena, ex);
            }
            return dt;
        }

        #endregion



        public string NombreSP(string modulo)
        {
            string sp = "";
            switch (modulo)
            {
                case "Diot":
                    sp = "SICEWEB_INFORME_ANALISIS_DIOT";
                    break;
                case "ReporteAnual":
                    sp = "SICEWEB_INFORME_ANALISIS_REPORTE_ANUAL";
                    break;
                case "DTA":
                    sp = "SICEWEB_INFORME_ANALISIS_DTA";
                    break;
                case "Compulsa":
                    sp = "SICEWEB_INFORME_ANALISIS_COMPULSA_DSVUCEM";
                    break;
                case "CG":
                    sp = "SICEWEB_INFORME_ANALISIS_CG";
                    break;
                case "CreaHC":
                    sp = "creaHojaDeCalculo";
                    break;
                case "ConsultaHC_HCKEY":
                    sp = "HOJACALCULO_HCKEY_SELECT";
                    break;
                case "ConsultaHC_HCKEY_FACTURA":
                    sp = "HOJACALCULO_HCKEY_FACTURA_SELECT";
                    break;
                case "ConsultaHC":
                    sp = "HOJACALCULO_SELECT";
                    break;
                case "ValidaHCDF":
                    sp = "HCDF_VALIDA";
                    break;
                case "ActualizaHC":
                    sp = "HOJACALCULO_UPDATE";
                    break;
                case "ActualizaHCFactura":
                    sp = "HOJACALCULO_HCKEY_FACTURA_UPDATE";
                    break;                    
                case "EliminaHC":
                    sp = "HOJACALCULO_DELETE";
                    break;
            }
            return sp;
        }

    }
}