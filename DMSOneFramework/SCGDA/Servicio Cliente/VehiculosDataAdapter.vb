Option Strict On
Option Explicit On

Namespace SCGDataAccess
    Public Class VehiculosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"

        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strDescModelo As String = "DescModelo"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strAnoVehiculo As String = "AnoVehiculo"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strCodigoColor As String = "CodigoColor"
        Private Const mc_strColor As String = "Color"
        Private Const mc_strCodColorTap As String = "CodColorTap"
        Private Const mc_strVIN As String = "VIN"
        Private Const mc_strNum_Motor As String = "Num_Motor"
        Private Const mc_strCodMarcaMotor As String = "CodMarcaMotor"
        Private Const mc_strCant_Pasajeros As String = "Cant_Pasajeros"
        Private Const mc_strCodigoUbicacion As String = "CodigoUbicacion"
        Private Const mc_strTipo As String = "Tipo"
        Private Const mc_strCodEstatus As String = "CodEstatus"
        Private Const mc_strTipoTraccion As String = "TipoTraccion"
        Private Const mc_strNumeroCilindros As String = "NumeroCilindros"
        Private Const mc_strTipoTecho As String = "TipoTecho"
        Private Const mc_strCodCarroceria As String = "CodCarroceria"
        Private Const mc_strCantidadPuertas As String = "CantidadPuertas"
        Private Const mc_strPeso As String = "Peso"
        Private Const mc_strCilindrada As String = "Cilindrada"
        Private Const mc_strCodCategoria As String = "CodCategoria"
        Private Const mc_strCodCombustible As String = "CodCombustible"
        Private Const mc_strCantEjes As String = "CantEjes"
        Private Const mc_strIDVehiculo As String = "IDVehiculo"
        Private Const mc_strCodTipoCabina As String = "CodTipoCabina"
        Private Const mc_strPotencia As String = "Potencia"
        Private Const mc_strCodTransmision As String = "CodTransmision"
        Private Const mc_strAccesorios As String = "Accesorios"
        Private Const mc_strGarantiaKM As String = "GarantiaKM"
        Private Const mc_strCliente As String = "Cliente"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strGarantiaTM As String = "GarantiaTM"
        Private Const mc_strFechaVenta As String = "FechaVenta"
        Private Const mc_strFechaUltServicio As String = "FechaUltimoServicio"
        Private Const mc_strFechaPxServicio As String = "FechaProximoServicio"
        Private Const mc_strFechaReserva As String = "FechaReserva"
        Private Const mc_strFechaVcReserva As String = "FechaVencimientoReserva"
        Private Const mc_strNoPedidoFab As String = "NoPedidoFabrica"

        Private Const mc_strSCGTA_SP_SELCabinas As String = "SCGTA_SP_SELCabinas"
        Private Const mc_strSCGTA_SP_SELCarroceria As String = "SCGTA_SP_SELCarroceria"
        Private Const mc_strSCGTA_SP_SELColor As String = "SCGTA_SP_SELColor"
        Private Const mc_strSCGTA_SP_SELCombustible As String = "SCGTA_SP_SELCombustible"
        Private Const mc_strSCGTA_SP_SELMarca_Motor As String = "SCGTA_SP_SELMarca_Motor"
        Private Const mc_strSCGTA_SP_SELTecho As String = "SCGTA_SP_SELTecho"
        Private Const mc_strSCGTA_SP_SELTraccion As String = "SCGTA_SP_SELTraccion"
        Private Const mc_strSCGTA_SP_SELTransmision As String = "SCGTA_SP_SELTransmision"
        Private Const mc_strSCGTA_SP_SELUbicaciones As String = "SCGTA_SP_SELUbicaciones"
        Private Const mc_strSCGTA_SP_SELCategorias As String = "SCGTA_SP_SELCategorias"
        Private Const mc_strSCGTA_SP_SELEstadoVehiculo As String = "SCGTA_SP_SELEstadoVehiculo"
        Private Const mc_strSCGTA_SP_SELTipoVehiculo As String = "SCGTA_SP_SELTipoVehiculo"
        Private Const mc_strSCGTA_SP_InsVehiculo As String = "SCGTA_SP_InsVehiculo"
        Private Const mc_strSCGTA_SP_UpdVehiculo As String = "SCGTA_SP_UpdVehiculo"
        Private Const mc_strSCGTA_SP_SELVehiculos As String = "SCGTA_SP_SELVehiculos"

        Private Const mc_strArroba As String = "@"

#End Region

#Region "Objetos"

        Private m_adpVehiculos As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializa ClientesDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpVehiculos = New SqlClient.SqlDataAdapter

        End Sub

#End Region

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Return Nothing
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Return Nothing
        End Function

        Public Property MissingMappingAction() As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction

            Get

            End Get

            Set(ByVal Value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal Value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings() As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get
                Return Nothing
            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByRef dataSet As VehiculosDataset, _
                                       Optional ByVal p_intIDVehiculo As Integer = -1, _
                                       Optional ByVal p_strPlaca As String = "", _
                                       Optional ByVal p_strNoVehiculo As String = "") As Integer

            Try

                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpVehiculos.SelectCommand = CreateSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado

                If p_intIDVehiculo <> -1 Then
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strIDVehiculo).Value = p_intIDVehiculo
                Else
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strIDVehiculo).Value = DBNull.Value
                End If

                If p_strPlaca <> "" Then
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = p_strPlaca
                Else
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = DBNull.Value
                End If

                If p_strNoVehiculo <> "" Then
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strNoVehiculo).Value = p_strNoVehiculo
                Else
                    m_adpVehiculos.SelectCommand.Parameters(mc_strArroba & mc_strNoVehiculo).Value = DBNull.Value
                End If

                m_adpVehiculos.SelectCommand.Connection = m_cnnSCGTaller


                Call m_adpVehiculos.Fill(dataSet.SCGTA_VW_Vehiculos)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As VehiculosDataset) As Integer

            Try

                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
                If m_cnnSCGTaller.ConnectionString = "" Then
                    m_cnnSCGTaller.ConnectionString = strConexionADO
                End If
                Call m_cnnSCGTaller.Open()


                m_adpVehiculos.InsertCommand = CreateInsertCommand()
                m_adpVehiculos.InsertCommand.Connection = m_cnnSCGTaller

                m_adpVehiculos.UpdateCommand = CreateUpdateCommand()
                m_adpVehiculos.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpVehiculos.Update(dataSet.SCGTA_VW_Vehiculos)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CreateSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand()
            cmdSel.CommandType = CommandType.StoredProcedure
            cmdSel.CommandText = mc_strSCGTA_SP_SELVehiculos
            With cmdSel

                .Parameters.Add(mc_strArroba & mc_strIDVehiculo, SqlDbType.Int, 4, mc_strIDVehiculo)
                .Parameters.Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 50, mc_strPlaca)
                .Parameters.Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 50, mc_strNoVehiculo)

            End With

            Return cmdSel

        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Dim cmdIns As New SqlClient.SqlCommand
            cmdIns.CommandType = CommandType.StoredProcedure
            cmdIns.CommandText = mc_strSCGTA_SP_InsVehiculo
            With cmdIns

                .Parameters.Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 20, mc_strNoVehiculo)
                .Parameters.Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8, mc_strCodMarca)
                .Parameters.Add(mc_strArroba & mc_strDescMarca, SqlDbType.NVarChar, 30, mc_strDescMarca)
                .Parameters.Add(mc_strArroba & mc_strCodModelo, SqlDbType.NVarChar, 8, mc_strCodModelo)
                .Parameters.Add(mc_strArroba & mc_strDescModelo, SqlDbType.NVarChar, 30, mc_strDescModelo)
                .Parameters.Add(mc_strArroba & mc_strCodEstilo, SqlDbType.NVarChar, 8, mc_strCodEstilo)
                .Parameters.Add(mc_strArroba & mc_strDescEstilo, SqlDbType.NVarChar, 30, mc_strDescEstilo)
                .Parameters.Add(mc_strArroba & mc_strAnoVehiculo, SqlDbType.Int, 4, mc_strAnoVehiculo)
                .Parameters.Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 20, mc_strPlaca)
                .Parameters.Add(mc_strArroba & mc_strCodigoColor, SqlDbType.NVarChar, 8, mc_strCodigoColor)
                .Parameters.Add(mc_strArroba & mc_strColor, SqlDbType.NVarChar, 30, mc_strColor)
                .Parameters.Add(mc_strArroba & mc_strCodColorTap, SqlDbType.SmallInt, 2, mc_strCodColorTap)
                .Parameters.Add(mc_strArroba & mc_strVIN, SqlDbType.NVarChar, 50, mc_strVIN)
                .Parameters.Add(mc_strArroba & mc_strNum_Motor, SqlDbType.NVarChar, 50, mc_strNum_Motor)
                .Parameters.Add(mc_strArroba & mc_strCodMarcaMotor, SqlDbType.SmallInt, 2, mc_strCodMarcaMotor)
                .Parameters.Add(mc_strArroba & mc_strCant_Pasajeros, SqlDbType.SmallInt, 2, mc_strCant_Pasajeros)
                .Parameters.Add(mc_strArroba & mc_strCodigoUbicacion, SqlDbType.SmallInt, 2, mc_strCodigoUbicacion)
                .Parameters.Add(mc_strArroba & mc_strTipo, SqlDbType.SmallInt, 2, mc_strTipo)
                .Parameters.Add(mc_strArroba & mc_strCodEstatus, SqlDbType.SmallInt, 2, mc_strCodEstatus)
                .Parameters.Add(mc_strArroba & mc_strTipoTraccion, SqlDbType.SmallInt, 2, mc_strTipoTraccion)
                .Parameters.Add(mc_strArroba & mc_strNumeroCilindros, SqlDbType.SmallInt, 2, mc_strNumeroCilindros)
                .Parameters.Add(mc_strArroba & mc_strTipoTecho, SqlDbType.SmallInt, 2, mc_strTipoTecho)
                .Parameters.Add(mc_strArroba & mc_strCodCarroceria, SqlDbType.SmallInt, 2, mc_strCodCarroceria)
                .Parameters.Add(mc_strArroba & mc_strCantidadPuertas, SqlDbType.SmallInt, 2, mc_strCantidadPuertas)
                .Parameters.Add(mc_strArroba & mc_strPeso, SqlDbType.Int, 4, mc_strPeso)
                .Parameters.Add(mc_strArroba & mc_strCilindrada, SqlDbType.Int, 2, mc_strCilindrada)
                .Parameters.Add(mc_strArroba & mc_strCodCategoria, SqlDbType.SmallInt, 2, mc_strCodCategoria)
                .Parameters.Add(mc_strArroba & mc_strCodCombustible, SqlDbType.SmallInt, 2, mc_strCodCombustible)
                .Parameters.Add(mc_strArroba & mc_strCantEjes, SqlDbType.SmallInt, 2, mc_strCantEjes)
                .Parameters.Add(mc_strArroba & mc_strCodTipoCabina, SqlDbType.SmallInt, 2, mc_strCodTipoCabina)
                .Parameters.Add(mc_strArroba & mc_strPotencia, SqlDbType.Int, 4, mc_strPotencia)
                .Parameters.Add(mc_strArroba & mc_strCodTransmision, SqlDbType.SmallInt, 2, mc_strCodTransmision)
                .Parameters.Add(mc_strArroba & mc_strAccesorios, SqlDbType.NVarChar, 50, mc_strAccesorios)
                .Parameters.Add(mc_strArroba & mc_strGarantiaKM, SqlDbType.Int, 4, mc_strGarantiaKM)
                .Parameters.Add(mc_strArroba & mc_strCliente, SqlDbType.NVarChar, 100, mc_strCliente)
                .Parameters.Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 15, mc_strCardCode)
                .Parameters.Add(mc_strArroba & mc_strGarantiaTM, SqlDbType.SmallInt, 2, mc_strGarantiaTM)
                .Parameters.Add(mc_strArroba & mc_strFechaVenta, SqlDbType.DateTime, 9, mc_strFechaVenta)
                .Parameters.Add(mc_strArroba & mc_strIDVehiculo, SqlDbType.NVarChar, 8, mc_strIDVehiculo).Direction = ParameterDirection.Output

                .Parameters.Add(mc_strArroba & mc_strFechaPxServicio, SqlDbType.DateTime, 9, mc_strFechaPxServicio)
                .Parameters.Add(mc_strArroba & mc_strFechaUltServicio, SqlDbType.DateTime, 9, mc_strFechaUltServicio)
                .Parameters.Add(mc_strArroba & mc_strFechaReserva, SqlDbType.DateTime, 9, mc_strFechaReserva)
                .Parameters.Add(mc_strArroba & mc_strFechaVcReserva, SqlDbType.DateTime, 9, mc_strFechaVcReserva)
                .Parameters.Add(mc_strArroba & mc_strNoPedidoFab, SqlDbType.NVarChar, 100, mc_strNoPedidoFab)
            End With

            Return cmdIns

        End Function

        Private Function CreateUpdateCommand() As SqlClient.SqlCommand

            Dim cmdUpd As New SqlClient.SqlCommand
            cmdUpd.CommandType = CommandType.StoredProcedure
            cmdUpd.CommandText = mc_strSCGTA_SP_UpdVehiculo
            With cmdUpd
                .Parameters.Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 20, mc_strNoVehiculo)
                .Parameters.Add(mc_strArroba & mc_strCodMarca, SqlDbType.NVarChar, 8, mc_strCodMarca)
                .Parameters.Add(mc_strArroba & mc_strDescMarca, SqlDbType.NVarChar, 30, mc_strDescMarca)
                .Parameters.Add(mc_strArroba & mc_strCodModelo, SqlDbType.NVarChar, 8, mc_strCodModelo)
                .Parameters.Add(mc_strArroba & mc_strCodEstilo, SqlDbType.NVarChar, 8, mc_strCodEstilo)
                .Parameters.Add(mc_strArroba & mc_strDescEstilo, SqlDbType.NVarChar, 30, mc_strDescEstilo)
                .Parameters.Add(mc_strArroba & mc_strAnoVehiculo, SqlDbType.Int, 4, mc_strAnoVehiculo)
                .Parameters.Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 20, mc_strPlaca)
                .Parameters.Add(mc_strArroba & mc_strCodigoColor, SqlDbType.NVarChar, 8, mc_strCodigoColor)
                .Parameters.Add(mc_strArroba & mc_strColor, SqlDbType.NVarChar, 30, mc_strColor)
                .Parameters.Add(mc_strArroba & mc_strCodColorTap, SqlDbType.SmallInt, 2, mc_strCodColorTap)
                .Parameters.Add(mc_strArroba & mc_strVIN, SqlDbType.NVarChar, 50, mc_strVIN)
                .Parameters.Add(mc_strArroba & mc_strNum_Motor, SqlDbType.NVarChar, 50, mc_strNum_Motor)
                .Parameters.Add(mc_strArroba & mc_strCodMarcaMotor, SqlDbType.SmallInt, 2, mc_strCodMarcaMotor)
                .Parameters.Add(mc_strArroba & mc_strCant_Pasajeros, SqlDbType.SmallInt, 2, mc_strCant_Pasajeros)
                .Parameters.Add(mc_strArroba & mc_strCodigoUbicacion, SqlDbType.SmallInt, 2, mc_strCodigoUbicacion)
                .Parameters.Add(mc_strArroba & mc_strTipo, SqlDbType.SmallInt, 2, mc_strTipo)
                .Parameters.Add(mc_strArroba & mc_strCodEstatus, SqlDbType.SmallInt, 2, mc_strCodEstatus)
                .Parameters.Add(mc_strArroba & mc_strTipoTraccion, SqlDbType.SmallInt, 2, mc_strTipoTraccion)
                .Parameters.Add(mc_strArroba & mc_strNumeroCilindros, SqlDbType.SmallInt, 2, mc_strNumeroCilindros)
                .Parameters.Add(mc_strArroba & mc_strTipoTecho, SqlDbType.SmallInt, 2, mc_strTipoTecho)
                .Parameters.Add(mc_strArroba & mc_strCodCarroceria, SqlDbType.SmallInt, 2, mc_strCodCarroceria)
                .Parameters.Add(mc_strArroba & mc_strCantidadPuertas, SqlDbType.SmallInt, 2, mc_strCantidadPuertas)
                .Parameters.Add(mc_strArroba & mc_strPeso, SqlDbType.Int, 4, mc_strPeso)
                .Parameters.Add(mc_strArroba & mc_strCilindrada, SqlDbType.Int, 2, mc_strCilindrada)
                .Parameters.Add(mc_strArroba & mc_strCodCategoria, SqlDbType.SmallInt, 2, mc_strCodCategoria)
                .Parameters.Add(mc_strArroba & mc_strCodCombustible, SqlDbType.SmallInt, 2, mc_strCodCombustible)
                .Parameters.Add(mc_strArroba & mc_strCantEjes, SqlDbType.SmallInt, 2, mc_strCantEjes)
                .Parameters.Add(mc_strArroba & mc_strCodTipoCabina, SqlDbType.SmallInt, 2, mc_strCodTipoCabina)
                .Parameters.Add(mc_strArroba & mc_strPotencia, SqlDbType.Int, 4, mc_strPotencia)
                .Parameters.Add(mc_strArroba & mc_strCodTransmision, SqlDbType.SmallInt, 2, mc_strCodTransmision)
                .Parameters.Add(mc_strArroba & mc_strAccesorios, SqlDbType.NVarChar, 50, mc_strAccesorios)
                .Parameters.Add(mc_strArroba & mc_strGarantiaKM, SqlDbType.Int, 4, mc_strGarantiaKM)
                .Parameters.Add(mc_strArroba & mc_strCliente, SqlDbType.NVarChar, 100, mc_strCliente)
                .Parameters.Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 15, mc_strCardCode)
                .Parameters.Add(mc_strArroba & mc_strGarantiaTM, SqlDbType.SmallInt, 2, mc_strGarantiaTM)
                .Parameters.Add(mc_strArroba & mc_strIDVehiculo, SqlDbType.NVarChar, 8, mc_strIDVehiculo)
                .Parameters.Add(mc_strArroba & mc_strFechaVenta, SqlDbType.DateTime, 9, mc_strFechaVenta)

                .Parameters.Add(mc_strArroba & mc_strFechaPxServicio, SqlDbType.DateTime, 9, mc_strFechaPxServicio)
                .Parameters.Add(mc_strArroba & mc_strFechaUltServicio, SqlDbType.DateTime, 9, mc_strFechaUltServicio)
                .Parameters.Add(mc_strArroba & mc_strFechaReserva, SqlDbType.DateTime, 9, mc_strFechaReserva)
                .Parameters.Add(mc_strArroba & mc_strFechaVcReserva, SqlDbType.DateTime, 9, mc_strFechaVcReserva)
                .Parameters.Add(mc_strArroba & mc_strNoPedidoFab, SqlDbType.NVarChar, 100, mc_strNoPedidoFab)

            End With

            Return cmdUpd

        End Function

#End Region

#Region "Fills Catalogos"

        Public Function FillReaderCabinas() As SqlClient.SqlDataReader

            Dim drdDatosCabinas As SqlClient.SqlDataReader
            Dim cmdDatosCabinas As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosCabinas

                .CommandText = mc_strSCGTA_SP_SELCabinas
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosCabinas = .ExecuteReader()

            End With

            Return drdDatosCabinas

        End Function

        Public Function FillReaderCarroceria() As SqlClient.SqlDataReader

            Dim drdDatosCarroceria As SqlClient.SqlDataReader
            Dim cmdDatosCarroceria As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosCarroceria

                .CommandText = mc_strSCGTA_SP_SELCarroceria
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosCarroceria = .ExecuteReader()

            End With

            Return drdDatosCarroceria

        End Function

        Public Function FillReaderColor() As SqlClient.SqlDataReader

            Dim drdDatosColor As SqlClient.SqlDataReader
            Dim cmdDatosColor As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosColor

                .CommandText = mc_strSCGTA_SP_SELColor
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosColor = .ExecuteReader()

            End With

            Return drdDatosColor

        End Function

        Public Function FillReaderCombustible() As SqlClient.SqlDataReader

            Dim drdDatosCombustible As SqlClient.SqlDataReader
            Dim cmdDatosCombustible As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosCombustible

                .CommandText = mc_strSCGTA_SP_SELCombustible
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosCombustible = .ExecuteReader()

            End With

            Return drdDatosCombustible

        End Function

        Public Function FillReaderMarca_Motor() As SqlClient.SqlDataReader

            Dim drdDatosMarca_Motor As SqlClient.SqlDataReader
            Dim cmdDatosMarca_Motor As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosMarca_Motor

                .CommandText = mc_strSCGTA_SP_SELMarca_Motor
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosMarca_Motor = .ExecuteReader()

            End With

            Return drdDatosMarca_Motor

        End Function

        Public Function FillReaderTecho() As SqlClient.SqlDataReader

            Dim drdDatosTecho As SqlClient.SqlDataReader
            Dim cmdDatosTecho As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosTecho

                .CommandText = mc_strSCGTA_SP_SELTecho
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosTecho = .ExecuteReader()

            End With

            Return drdDatosTecho

        End Function

        Public Function FillReaderTraccion() As SqlClient.SqlDataReader

            Dim drdDatosTraccion As SqlClient.SqlDataReader
            Dim cmdDatosTraccion As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosTraccion

                .CommandText = mc_strSCGTA_SP_SELTraccion
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosTraccion = .ExecuteReader()

            End With

            Return drdDatosTraccion

        End Function

        Public Function FillReaderTransmision() As SqlClient.SqlDataReader

            Dim drdDatosTransmision As SqlClient.SqlDataReader
            Dim cmdDatosTransmision As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosTransmision

                .CommandText = mc_strSCGTA_SP_SELTransmision
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosTransmision = .ExecuteReader()

            End With

            Return drdDatosTransmision

        End Function

        Public Function FillReaderUbicaciones() As SqlClient.SqlDataReader

            Dim drdDatosUbicaciones As SqlClient.SqlDataReader
            Dim cmdDatosUbicaciones As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosUbicaciones

                .CommandText = mc_strSCGTA_SP_SELUbicaciones
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosUbicaciones = .ExecuteReader()

            End With

            Return drdDatosUbicaciones

        End Function

        Public Function FillReaderCategorias() As SqlClient.SqlDataReader

            Dim drdDatosCategorias As SqlClient.SqlDataReader
            Dim cmdDatosCategorias As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosCategorias

                .CommandText = mc_strSCGTA_SP_SELCategorias
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosCategorias = .ExecuteReader()

            End With

            Return drdDatosCategorias

        End Function

        Public Function FillReaderEstadoVehiculo() As SqlClient.SqlDataReader

            Dim drdDatosEstadoVehiculo As SqlClient.SqlDataReader
            Dim cmdDatosEstadoVehiculo As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosEstadoVehiculo

                .CommandText = mc_strSCGTA_SP_SELEstadoVehiculo
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosEstadoVehiculo = .ExecuteReader()

            End With

            Return drdDatosEstadoVehiculo

        End Function

        Public Function FillReaderTipoVehiculo() As SqlClient.SqlDataReader

            Dim drdDatosTipoVehiculo As SqlClient.SqlDataReader
            Dim cmdDatosTipoVehiculo As New SqlClient.SqlCommand()

            If m_cnnSCGTaller IsNot Nothing Then
                If m_cnnSCGTaller.State <> ConnectionState.Closed Then
                    m_cnnSCGTaller.Close()
                End If
            End If
            m_cnnSCGTaller.Open()

            With cmdDatosTipoVehiculo

                .CommandText = mc_strSCGTA_SP_SELTipoVehiculo
                .CommandType = CommandType.StoredProcedure
                .Connection = m_cnnSCGTaller

                drdDatosTipoVehiculo = .ExecuteReader()

            End With

            Return drdDatosTipoVehiculo

        End Function

#End Region

    End Class

End Namespace
