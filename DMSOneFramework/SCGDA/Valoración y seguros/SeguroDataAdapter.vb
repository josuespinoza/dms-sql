Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class SeguroDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_NoSeguro As String = "NoSeguro"
        Private Const mc_CodAgencia As String = "CodAgencia"
        Private Const mc_CodDeducible As String = "CodDeducible"
        Private Const mc_CodCobertura As String = "CodCobertura"
        Private Const mc_NoCaso As String = "NoCaso"
        Private Const mc_NoPoliza As String = "NoPoliza"
        Private Const mc_NoReferencia As String = "NoReferencia"
        Private Const mc_MontoAsegurado As String = "MontoAsegurado"
        Private Const mc_ValorReal As String = "ValorRealVehiculo"
        Private Const mc_Infraseguro As String = "Infraseguro"
        Private Const mc_AcreedorPrendario As String = "AcreedorPrendario"
        Private Const mc_VigenciaInicio As String = "VigenciaInicio"
        Private Const mc_VigenciaFin As String = "VigenciaFin"
        Private Const mc_Perito As String = "Perito"
        Private Const mc_FechaAccidente As String = "FechaAccidente"
        Private Const mc_FechaAvaluo As String = "FechaAvaluo"
        Private Const mc_MontoAvaluo As String = "MontoAvaluo"
        Private Const mc_Solicitud As String = "Solicitud"
        Private Const mc_DetalleGeneral As String = "DetalleGeneral"
        Private Const mc_DetalleAvaluo As String = "DetalleAvaluo"
        Private Const mc_MontoManoObraAdicional As String = "MontoManoObraAdicional"
        Private Const mc_MontoRepuestoAdicional As String = "MontoRepuestoAdicional"
        Private Const mc_FechaSolicitudAdicional As String = "FechaSolicitudAdicional"
        Private Const mc_Ajustador As String = "Ajustador"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_intCono As String = "Cono"
        Private Const mc_intcodMarca As String = "CodMarca"
        Private Const mc_strEstado As String = "Estado"
        Private Const mc_strIndicador As String = "indicador"
        Private Const mc_intCodAgencia As String = "CodAgencia"
        Private Const mc_strAvaluo As String = "noAvaluo"
        Private Const mc_MontoSuministros As String = "MontoSuministros"
        Private Const mc_MontoMaterialesPintura As String = "MontoMaterialPintura"
        Private Const mc_MontoMaterialesTaller As String = "MontoMaterialTaller"
        Private Const mc_MontoManoObra As String = "MontoManoObra"


        Private m_adpSeguro As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDSeguro As String = "SCGTA_SP_UpdSeguro"
        Private Const mc_strSCGTA_SP_SELSeguro As String = "SCGTA_SP_SelSeguro"
        Private Const mc_strSCGTA_SP_INSSeguro As String = "SCGTA_SP_InsSeguro"
        Private Const mc_strSCGTA_SP_DelSeguro As String = "SCGTA_SP_DelSeguro"
        Private Const mc_strSCGTA_SP_SelSeguroxOrden As String = "SCGTA_SP_SelSeguroxOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion
   
#End Region

#Region "Inicializa SegurosDataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSeguro = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

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

            End Get
        End Property

#End Region


#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As SeguroDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSeguro.SelectCommand = CrearSelectCommand()

                m_adpSeguro.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpSeguro.Fill(dataSet.SCGTA_TB_Seguro)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As SeguroDataset, _
                                       ByVal NoOrden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpSeguro.SelectCommand = CrearSelectCommandSelSeguroxOrden()

                m_adpSeguro.SelectCommand.Connection = m_cnnSCGTaller

                m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrden

                Call m_adpSeguro.Fill(dataSet.SCGTA_TB_Seguro)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As SeguroDataset, ByVal noorden As String, ByVal placa As String, _
                                      ByVal cono As Integer, ByVal codmarca As Integer, ByVal estado As String, _
                                      ByVal indicador As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If
                'Creacion del comando
                m_adpSeguro.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If noorden = "" Then
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = noorden
                End If

                If placa = "" Then

                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = placa
                End If

                If cono = 0 Then
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_intCono).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_intCono).Value = cono
                End If

                If codmarca = 0 Then

                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_intcodMarca).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_intcodMarca).Value = codmarca
                End If

                If estado = "" Then

                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strEstado).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strEstado).Value = estado
                End If

                If indicador = "" Then
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strIndicador).Value = System.DBNull.Value
                Else
                    m_adpSeguro.SelectCommand.Parameters(mc_strArroba & mc_strIndicador).Value = indicador
                End If

                m_adpSeguro.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpSeguro.Fill(dataSet.SCGTA_TB_Seguro)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As SeguroDataset) As Integer

            Try
                m_adpSeguro.InsertCommand = CreateInsertCommand()
                m_adpSeguro.InsertCommand.Connection = m_cnnSCGTaller

                m_adpSeguro.UpdateCommand = CrearUpdateCommand()
                m_adpSeguro.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpSeguro.Update(dataSet.SCGTA_TB_Seguro)

            Catch ex As Exception

                Throw ex

            End Try


        End Function


#End Region


#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELSeguro)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.VarChar, 20, mc_strPlaca)

                    .Add(mc_strArroba & mc_intCono, SqlDbType.Int, 4, mc_intCono)

                    .Add(mc_strArroba & mc_intcodMarca, SqlDbType.Int, 9, mc_intcodMarca)

                    .Add(mc_strArroba & mc_strEstado, SqlDbType.VarChar, 50, mc_strEstado)

                    .Add(mc_strArroba & mc_strIndicador, SqlDbType.VarChar, 1, mc_strIndicador)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearSelectCommandSelSeguroxOrden() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelSeguroxOrden)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Private Function CrearSelectCommandFiltro() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELSeguro)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSSeguro)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intCodAgencia, SqlDbType.Int, 4, mc_intCodAgencia)

                    .Add(mc_strArroba & mc_CodDeducible, SqlDbType.Char, 18, mc_CodDeducible)

                    .Add(mc_strArroba & mc_CodCobertura, SqlDbType.Int, 4, mc_CodCobertura)

                    .Add(mc_strArroba & mc_NoCaso, SqlDbType.VarChar, 100, mc_NoCaso)

                    .Add(mc_strArroba & mc_NoPoliza, SqlDbType.VarChar, 100, mc_NoPoliza)

                    .Add(mc_strArroba & mc_NoReferencia, SqlDbType.VarChar, 100, mc_NoReferencia)

                    .Add(mc_strArroba & mc_MontoAsegurado, SqlDbType.Money, 8, mc_MontoAsegurado)

                    .Add(mc_strArroba & mc_ValorReal, SqlDbType.Decimal, 12, mc_ValorReal)

                    .Add(mc_strArroba & mc_Infraseguro, SqlDbType.VarChar, 100, mc_Infraseguro)

                    .Add(mc_strArroba & mc_AcreedorPrendario, SqlDbType.VarChar, 100, mc_AcreedorPrendario)

                    .Add(mc_strArroba & mc_VigenciaInicio, SqlDbType.DateTime, 8, mc_VigenciaInicio)

                    .Add(mc_strArroba & mc_VigenciaFin, SqlDbType.DateTime, 8, mc_VigenciaFin)

                    .Add(mc_strArroba & mc_Perito, SqlDbType.VarChar, 100, mc_Perito)

                    .Add(mc_strArroba & mc_Ajustador, SqlDbType.VarChar, 100, mc_Ajustador)

                    .Add(mc_strArroba & mc_FechaAccidente, SqlDbType.DateTime, 8, mc_FechaAccidente)

                    .Add(mc_strArroba & mc_FechaAvaluo, SqlDbType.DateTime, 8, mc_FechaAvaluo)

                    .Add(mc_strArroba & mc_MontoAvaluo, SqlDbType.Money, 8, mc_MontoAvaluo)

                    .Add(mc_strArroba & mc_Solicitud, SqlDbType.DateTime, 8, mc_Solicitud)

                    .Add(mc_strArroba & mc_DetalleGeneral, SqlDbType.VarChar, 500, mc_DetalleGeneral)

                    .Add(mc_strArroba & mc_DetalleAvaluo, SqlDbType.VarChar, 500, mc_DetalleAvaluo)

                    .Add(mc_strArroba & mc_MontoManoObraAdicional, SqlDbType.Money, 8, mc_MontoManoObraAdicional)

                    .Add(mc_strArroba & mc_MontoRepuestoAdicional, SqlDbType.Money, 8, mc_MontoRepuestoAdicional)

                    .Add(mc_strArroba & mc_FechaSolicitudAdicional, SqlDbType.SmallDateTime, 8, mc_FechaSolicitudAdicional)

                    .Add(mc_strArroba & mc_strAvaluo, SqlDbType.VarChar, 100, mc_strAvaluo)

                    .Add(mc_strArroba & mc_MontoManoObra, SqlDbType.Decimal, 9, mc_MontoManoObra)

                    .Add(mc_strArroba & mc_MontoSuministros, SqlDbType.Decimal, 9, mc_MontoSuministros)

                    .Add(mc_strArroba & mc_MontoMaterialesPintura, SqlDbType.Decimal, 9, mc_MontoMaterialesPintura)

                    .Add(mc_strArroba & mc_MontoMaterialesTaller, SqlDbType.Decimal, 9, mc_MontoMaterialesTaller)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDSeguro)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_NoSeguro, SqlDbType.Int, 4, mc_NoSeguro)

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intCodAgencia, SqlDbType.Int, 4, mc_intCodAgencia)

                    .Add(mc_strArroba & mc_CodDeducible, SqlDbType.Char, 18, mc_CodDeducible)

                    .Add(mc_strArroba & mc_CodCobertura, SqlDbType.Int, 4, mc_CodCobertura)

                    .Add(mc_strArroba & mc_NoCaso, SqlDbType.VarChar, 100, mc_NoCaso)

                    .Add(mc_strArroba & mc_NoPoliza, SqlDbType.VarChar, 100, mc_NoPoliza)

                    .Add(mc_strArroba & mc_NoReferencia, SqlDbType.VarChar, 100, mc_NoReferencia)

                    .Add(mc_strArroba & mc_MontoAsegurado, SqlDbType.Money, 8, mc_MontoAsegurado)

                    .Add(mc_strArroba & mc_ValorReal, SqlDbType.Decimal, 12, mc_ValorReal)

                    .Add(mc_strArroba & mc_Infraseguro, SqlDbType.VarChar, 100, mc_Infraseguro)

                    .Add(mc_strArroba & mc_AcreedorPrendario, SqlDbType.VarChar, 100, mc_AcreedorPrendario)

                    .Add(mc_strArroba & mc_VigenciaInicio, SqlDbType.DateTime, 8, mc_VigenciaInicio)

                    .Add(mc_strArroba & mc_VigenciaFin, SqlDbType.DateTime, 8, mc_VigenciaFin)

                    .Add(mc_strArroba & mc_Perito, SqlDbType.VarChar, 100, mc_Perito)

                    .Add(mc_strArroba & mc_Ajustador, SqlDbType.VarChar, 100, mc_Ajustador)

                    .Add(mc_strArroba & mc_FechaAccidente, SqlDbType.DateTime, 8, mc_FechaAccidente)

                    .Add(mc_strArroba & mc_FechaAvaluo, SqlDbType.DateTime, 8, mc_FechaAvaluo)

                    .Add(mc_strArroba & mc_MontoAvaluo, SqlDbType.Money, 8, mc_MontoAvaluo)

                    .Add(mc_strArroba & mc_Solicitud, SqlDbType.DateTime, 8, mc_Solicitud)

                    .Add(mc_strArroba & mc_DetalleGeneral, SqlDbType.VarChar, 500, mc_DetalleGeneral)

                    .Add(mc_strArroba & mc_DetalleAvaluo, SqlDbType.VarChar, 500, mc_DetalleAvaluo)

                    .Add(mc_strArroba & mc_MontoManoObraAdicional, SqlDbType.Money, 8, mc_MontoManoObraAdicional)

                    .Add(mc_strArroba & mc_MontoRepuestoAdicional, SqlDbType.Money, 8, mc_MontoRepuestoAdicional)

                    .Add(mc_strArroba & mc_FechaSolicitudAdicional, SqlDbType.SmallDateTime, 8, mc_FechaSolicitudAdicional)

                    .Add(mc_strArroba & mc_strAvaluo, SqlDbType.VarChar, 100, mc_strAvaluo)

                    .Add(mc_strArroba & mc_MontoManoObra, SqlDbType.Decimal, 9, mc_MontoManoObra)

                    .Add(mc_strArroba & mc_MontoSuministros, SqlDbType.Decimal, 9, mc_MontoSuministros)

                    .Add(mc_strArroba & mc_MontoMaterialesPintura, SqlDbType.Decimal, 9, mc_MontoMaterialesPintura)

                    .Add(mc_strArroba & mc_MontoMaterialesTaller, SqlDbType.Decimal, 9, mc_MontoMaterialesTaller)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try


        End Function


#End Region

    End Class
End Namespace