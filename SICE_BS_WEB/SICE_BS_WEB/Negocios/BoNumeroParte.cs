using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SICE_BS_WEB.Negocios;
using System.Text;

namespace SICE_BS_WEB.Negocios
{
    public class BoNumeroParte
    {
        ControlExcepciones excepcion = new ControlExcepciones();

        public DataTable Consultar_NP(string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }



        public DataTable Consultar_NP_Detalle(string secuencia, string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_DETALLE_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
                        cmd.Parameters.Add("@SECUENCIA", SqlDbType.Int).Value = int.Parse(secuencia);
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
                throw ex;
            }
            return dt;
        }

        public DataTable Agregar_Detalle_NP(string cove, string factura, DateTime? fecha_factura, string num_parte, decimal cantidad, string umc, string descripcion,
                                            decimal valor_dolar, string clienteprov, string po, string secuencia, string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_DETALLE_AGREGAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@COVE", SqlDbType.VarChar).Value = cove;
                        cmd.Parameters.Add("@FACTURA", SqlDbType.VarChar).Value = factura;
                        cmd.Parameters.Add("@FECHA_FACTURA", SqlDbType.DateTime).Value = fecha_factura == null ? Convert.DBNull : fecha_factura;
                        cmd.Parameters.Add("@CODIGO_PRODUCTO", SqlDbType.VarChar).Value = num_parte;
                        cmd.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        cmd.Parameters.Add("@UNIDAD_MEDIDA", SqlDbType.VarChar).Value = umc;
                        cmd.Parameters.Add("@DESCRIPCION_COMERCIAL", SqlDbType.VarChar).Value = descripcion;
                        cmd.Parameters.Add("@PRECIO_TOTAL", SqlDbType.Decimal).Value = valor_dolar;
                        cmd.Parameters.Add("@CLIENTE_PROVEEDOR", SqlDbType.VarChar).Value = clienteprov;
                        cmd.Parameters.Add("@PO", SqlDbType.VarChar).Value = po;
                        cmd.Parameters.Add("@SECUENCIA", SqlDbType.VarChar).Value = secuencia;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }

        public DataTable Editar_Detalle_NP(Int64 key, string cove, string factura, DateTime? fecha_factura, string num_parte, decimal cantidad, string umc, string descripcion,
                                            decimal valor_dolar, string clienteprov, string po, string secuencia, string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_DETALLE_EDITAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DETALLENPKEY", SqlDbType.BigInt).Value = key;
                        cmd.Parameters.Add("@COVE", SqlDbType.VarChar).Value = cove;
                        cmd.Parameters.Add("@FACTURA", SqlDbType.VarChar).Value = factura;
                        cmd.Parameters.Add("@FECHA_FACTURA", SqlDbType.DateTime).Value = fecha_factura == null ? Convert.DBNull : fecha_factura;
                        cmd.Parameters.Add("@CODIGO_PRODUCTO", SqlDbType.VarChar).Value = num_parte;
                        cmd.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = cantidad;
                        cmd.Parameters.Add("@UNIDAD_MEDIDA", SqlDbType.VarChar).Value = umc;
                        cmd.Parameters.Add("@DESCRIPCION_COMERCIAL", SqlDbType.VarChar).Value = descripcion;
                        cmd.Parameters.Add("@PRECIO_TOTAL", SqlDbType.Decimal).Value = valor_dolar;
                        cmd.Parameters.Add("@CLIENTE_PROVEEDOR", SqlDbType.VarChar).Value = clienteprov;
                        cmd.Parameters.Add("@PO", SqlDbType.VarChar).Value = po;
                        cmd.Parameters.Add("@SECUENCIA", SqlDbType.VarChar).Value = secuencia;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }

        public DataTable Eliminar_Detalle_NP(Int64 key, string secuencia, string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_DETALLE_ELIMINAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DETALLENPKEY", SqlDbType.BigInt).Value = key;
                        cmd.Parameters.Add("@SECUENCIA", SqlDbType.VarChar).Value = secuencia;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }



        public DataTable Validar_NP(string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "VALIDANP";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }

        public DataTable Errores_Detalle_NP(string pedimento, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_NP_ERRORES_CONSULTAR";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTOARMADO", SqlDbType.VarChar).Value = pedimento;
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
                throw ex;
            }
            return dt;
        }


        public DataTable Trae_Categorias(string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_CATEGORIAS";

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
                throw ex;
            }
            return dt;
        }

        public DataTable MigrarA24(string pedimento, string descarga, string tipo, string cadena, ref string mensaje)
        {
            DataTable dt = new DataTable();
            string sp = "SICEWEB_ENVIA_IMMEX";

            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar).Value = pedimento;
                        cmd.Parameters.Add("@DESCARGA", SqlDbType.VarChar).Value = descarga;
                        cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = tipo;
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
                throw ex;
            }
            return dt;
        }

    }
}