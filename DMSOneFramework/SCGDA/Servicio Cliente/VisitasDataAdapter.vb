Option Strict On
Option Explicit On 

Namespace SCGDataAccess

    Public Class VisitasDataAdapter

        Implements IDataAdapter

#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strNoVisita As String = "NoVisita"
        Private Const mc_strCodEstado As String = "CodEstado"
        Private Const mc_strEstado As String = "CodEstado"
        Private Const mc_strCodModelo As String = "CodModelo"
        Private Const mc_strCodEstilo As String = "CodEstilo"
        Private Const mc_strCodMarca As String = "CodMarca"
        Private Const mc_strDescModelo As String = "DescModelo"
        Private Const mc_strDescEstilo As String = "DescEstilo"
        Private Const mc_strDescMarca As String = "DescMarca"
        Private Const mc_strIDVehiculo As String = "IDVehiculo"
        Private Const mc_strNoVehiculo As String = "NoVehiculo"
        Private Const mc_strFecha_apertura As String = "Fecha_apertura"
        Private Const mc_strFecha_compromiso As String = "Fecha_compromiso"
        Private Const mc_strFecha_cierre As String = "Fecha_cierre"
        Private Const mc_strAsesor As String = "Asesor"
        Private Const mc_strCono As String = "Cono"
        Private Const mc_strPlaca As String = "Placa"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strCotizacion As String = "Cotizacion"

        Private Const mc_strFecha_entrega As String = "Fecha_entrega"

        'Private Const mc_strNumVisita As String = "NoVisita"

        'Declaración de las variables que determinan el tipo de busqueda..con like % o sin %
        'Private mc_strSimbolo As String = "Simbolo"

        Private Const mc_strFecha_apertura_ini As String = "Fecha_apertura_ini"
        Private Const mc_strFecha_compromiso_ini As String = "Fecha_compromiso_ini"
        Private Const mc_strFecha_cierre_ini As String = "Fecha_cierre_ini"
        Private Const mc_strFecha_apertura_fin As String = "Fecha_apertura_fin"
        Private Const mc_strFecha_compromiso_fin As String = "Fecha_compromiso_fin"
        Private Const mc_strFecha_cierre_fin As String = "Fecha_cierre_fin"

        'Declaración de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UpdVisita As String = "SCGTA_SP_UpdVisita"
        Private Const mc_strSCGTA_SP_SelVisitas As String = "SCGTA_SP_SelVisitas"
        Private Const mc_strSCGTA_SP_InsVisita As String = "SCGTA_SP_InsVisita"
        Private Const mc_strSCGTA_SP_DelVisita As String = "SCGTA_SP_DelVisita"
        Private Const mc_strSCGTA_SP_SelVisitaOrden As String = "SCGTA_SP_SELVisitaOrdenes"
        Private Const mc_strSCGTA_SP_SelVisita2 As String = "SCGTA_SP_SELVisita2"
        Private Const mc_strSCGTA_SP_SelVisita3 As String = "SCGTA_SP_SelOrdenEncabezado"
        Private Const mc_strSCGTA_SP_UPDFechaCiereVisita As String = "SCGTA_SP_UPDFechaCierreVisita"
        Private Const mc_strSCGTA_SP_SELNumOTAbiertas As String = "SCGTA_SP_SELNumOTAbiertas"

        Private m_adpVisita As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region

#Region "Constructores"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpVisita = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)
            m_adpVisita = New SqlClient.SqlDataAdapter

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


        Public Overloads Function Fill(ByVal dataSet As VisitaDataset) As Integer
            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If
                m_adpVisita.SelectCommand = CrearSelectCommand()

                m_adpVisita.SelectCommand.Connection = m_cnnSCGTaller

                'Sirve para que no se ponga null en el datagrid, para ello se pone un default value.
                dataSet.SCGTA_TB_Visita.Fecha_cierreColumn.DefaultValue = "  /  /    "

                Call m_adpVisita.Fill(dataSet.SCGTA_TB_Visita)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Fill(ByVal dataSet As VisitaDataset, ByVal m_strCardCode As String, _
                                       ByVal m_strPlaca As String, ByVal m_intNoVisita As Integer, _
                                       ByVal m_intCodMarca As Integer, ByVal m_intCodModelo As Integer, _
                                       ByVal m_intCodEstilo As Integer, ByVal m_intCodEstadoVisita As Integer, _
                                       ByVal m_dtApertura_ini As Date, ByVal m_dtCierre_ini As Date, _
                                       ByVal m_dtCompromiso_ini As Date, ByVal m_dtApertura_fin As Date, _
                                       ByVal m_dtCierre_fin As Date, ByVal m_dtCompromiso_fin As Date, _
                                       ByVal m_strCono As String, ByVal m_strNoVehiculo As String, _
                                       ByVal m_strAsesor As String) As Integer

            m_adpVisita.SelectCommand = CrearSelectCommand()

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If


                '    '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If m_strCardCode <> "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = m_strCardCode
                End If

                If m_intNoVisita <> 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strNoVisita).Value = m_intNoVisita
                End If

                If m_intCodEstadoVisita <> 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strEstado).Value = m_intCodEstadoVisita
                End If

                If m_strPlaca <> "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = m_strPlaca
                End If

                If m_strNoVehiculo <> "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strNoVehiculo).Value = m_strNoVehiculo
                End If

                If m_intCodMarca <> 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = m_intCodMarca
                End If

                If m_intCodModelo <> 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = m_intCodModelo
                End If

                If m_intCodEstilo <> 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodEstilo).Value = m_intCodEstilo
                End If

                If m_dtApertura_ini = Nothing Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_apertura_ini).Value = System.DBNull.Value
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_apertura_fin).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_apertura_ini).Value = CDate(m_dtApertura_ini)
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_apertura_fin).Value = CDate(m_dtApertura_fin)
                End If


                If m_dtCompromiso_ini = Nothing Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_compromiso_ini).Value = System.DBNull.Value
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_compromiso_fin).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_compromiso_ini).Value = m_dtCompromiso_ini
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_compromiso_fin).Value = m_dtCompromiso_fin
                End If

                If m_dtCierre_ini = Nothing Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_cierre_ini).Value = System.DBNull.Value
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_cierre_fin).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_cierre_ini).Value = m_dtCierre_ini
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strFecha_cierre_fin).Value = m_dtCierre_fin
                End If

                If m_strCono <> "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = m_strCono
                End If

                If m_strAsesor <> "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strAsesor).Value = m_strAsesor
                End If

                m_adpVisita.SelectCommand.Connection = m_cnnSCGTaller

                'Sirve para que no se ponga null en el datagrid, para ello se pone un default value.
                dataSet.SCGTA_TB_Visita.Fecha_cierreColumn.DefaultValue = Today

                Call m_adpVisita.Fill(dataSet.SCGTA_TB_Visita)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As VisitaDataset, ByVal m_strCardCode As Integer, ByVal m_strPlaca As String, _
                                        ByVal m_strNoVisita As String, ByVal m_strCodMarca As String, ByVal m_intModelo As Integer, _
                                        ByVal m_intCono As Integer) As Integer

            m_adpVisita.SelectCommand = CrearSelectCommandOrden()

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando


                '    '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If m_strCardCode = Nothing Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = m_strCardCode
                End If


                If m_strNoVisita = "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strNoVisita).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strNoVisita).Value = m_strNoVisita
                End If


                If m_strPlaca = "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strPlaca).Value = m_strPlaca
                End If

                If m_intCono = 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCono).Value = m_intCono
                End If

                If m_strCodMarca = "" Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodMarca).Value = m_strCodMarca
                End If

                If m_intModelo = 0 Then
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = System.DBNull.Value
                Else
                    m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strCodModelo).Value = m_intModelo
                End If

                m_adpVisita.SelectCommand.Connection = m_cnnSCGTaller

                dataSet.SCGTA_TB_Visita.PlacaColumn.AllowDBNull = True
                dataSet.SCGTA_TB_Visita.NoVisitaColumn.AllowDBNull = True
                dataSet.SCGTA_TB_Visita.CodMarcaColumn.AllowDBNull = True
                dataSet.SCGTA_TB_Visita.CodModeloColumn.AllowDBNull = True

                Call m_adpVisita.Fill(dataSet.SCGTA_TB_Visita)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Fill(ByVal m_intNoVisita As Integer, ByVal dataSet As VisitaDataset) As Integer

            m_adpVisita.SelectCommand = CrearSelectCommandOrden2()

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpVisita.SelectCommand.Parameters(mc_strArroba & mc_strNoVisita).Value = m_intNoVisita

                m_adpVisita.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpVisita.Fill(dataSet.SCGTA_TB_Visita)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As VisitaDataset) As Integer


            Try
                Dim intNoVisita As Integer

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpVisita.InsertCommand = CreateInsertCommand()
                m_adpVisita.InsertCommand.Connection = m_cnnSCGTaller

                m_adpVisita.UpdateCommand = CrearUpdateCommand()
                m_adpVisita.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpVisita.Update(dataSet.SCGTA_TB_Visita)

                intNoVisita = CInt(m_adpVisita.InsertCommand.Parameters(mc_strArroba & mc_strNoVisita).Value)

                Return intNoVisita

            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Function Update(ByVal dataSet As VisitaDataset, _
                                         ByRef cn As SqlClient.SqlConnection, _
                                         ByRef tran As SqlClient.SqlTransaction, _
                                         Optional ByVal blnIniciar As Boolean = False, _
                                         Optional ByVal blnTerminar As Boolean = False) As Integer


            Try
                Dim intNoVisita As Integer
                If blnIniciar Then
                    cn = New SqlClient.SqlConnection
                    If m_cnnSCGTaller.State = ConnectionState.Closed Then
                        If m_cnnSCGTaller.ConnectionString = "" Then
                            m_cnnSCGTaller.ConnectionString = strConexionADO
                        End If
                        m_cnnSCGTaller.Open()
                        cn = m_cnnSCGTaller
                        tran = cn.BeginTransaction()
                    End If
                End If

                m_adpVisita.InsertCommand = CreateInsertCommand()
                m_adpVisita.InsertCommand.Connection = cn
                m_adpVisita.InsertCommand.Transaction = tran

                m_adpVisita.UpdateCommand = CrearUpdateCommand()
                m_adpVisita.UpdateCommand.Connection = cn
                m_adpVisita.UpdateCommand.Transaction = tran

                Call m_adpVisita.Update(dataSet.SCGTA_TB_Visita)

                intNoVisita = CInt(m_adpVisita.InsertCommand.Parameters(mc_strArroba & mc_strNoVisita).Value)

                Return intNoVisita
                If blnTerminar Then
                    tran.Commit()
                End If

            Catch ex As Exception

                Throw ex
            Finally
                'm_cnnSCGTaller.Close()

            End Try

        End Function

        Public Sub ActualizarFechaCierre(ByVal Codigo As Integer, ByVal Visita As Integer)

            Try
                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFechaCiereVisita)

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    ' .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Decimal, 9, mc_strNoVisita)
                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Decimal, 9, mc_strNoVisita).Value = Visita
                    '.Add(mc_strArroba & mc_strn, SqlDbType.Int, 4, mc_strCodigo).Value = Codigo

                End With


                cmdUPD.Connection = m_cnnSCGTaller
                cmdUPD.ExecuteNonQuery()



            Catch ex As Exception

                Throw ex

            Finally
                Call m_cnnSCGTaller.Close()

            End Try
        End Sub

        Public Function GetNumOTOpen(ByVal p_intNoVisita As Integer) As Integer
            Dim cmdOrdenes As SqlClient.SqlCommand
            Dim intNumResult As Integer

            cmdOrdenes = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELNumOTAbiertas)

            With cmdOrdenes

                .Connection = m_cnnSCGTaller
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 4).Value = p_intNoVisita

            End With

            intNumResult = CInt(cmdOrdenes.ExecuteScalar)

            m_cnnSCGTaller.Close()

            Return intNumResult

        End Function


#End Region

#Region "Creación de comandos"


        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelVisitas)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strFecha_apertura_ini, SqlDbType.DateTime, 10, mc_strFecha_apertura_fin)

                    .Add(mc_strArroba & mc_strFecha_compromiso_ini, SqlDbType.DateTime, 10, mc_strFecha_compromiso_fin)

                    .Add(mc_strArroba & mc_strFecha_cierre_ini, SqlDbType.DateTime, 10, mc_strFecha_cierre_fin)

                    .Add(mc_strArroba & mc_strFecha_apertura_fin, SqlDbType.DateTime, 10, mc_strFecha_apertura_fin)

                    .Add(mc_strArroba & mc_strFecha_compromiso_fin, SqlDbType.DateTime, 10, mc_strFecha_compromiso_fin)

                    .Add(mc_strArroba & mc_strFecha_cierre_fin, SqlDbType.DateTime, 10, mc_strFecha_cierre_fin)

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 50, mc_strCardCode)

                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 9, mc_strNoVisita)

                    .Add(mc_strArroba & mc_strCodEstado, SqlDbType.Int, 9, mc_strCodEstado)

                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.NVarChar, 20, mc_strPlaca)

                    .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.NVarChar, 50, mc_strNoVehiculo)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9, mc_strCodMarca)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Int, 9, mc_strCodModelo)

                    .Add(mc_strArroba & mc_strCodEstilo, SqlDbType.Int, 9, mc_strCodEstilo)

                    .Add(mc_strArroba & mc_strCono, SqlDbType.NVarChar, 50, mc_strCono)

                    .Add(mc_strArroba & mc_strAsesor, SqlDbType.Int, 9, mc_strAsesor)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearSelectCommandOrden() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelVisitaOrden)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 50, mc_strCardCode)

                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 9, mc_strNoVisita)

                    .Add(mc_strArroba & mc_strPlaca, SqlDbType.Int, 9, mc_strPlaca)

                    .Add(mc_strArroba & mc_strCono, SqlDbType.Int, 9, mc_strCono)

                    .Add(mc_strArroba & mc_strCodMarca, SqlDbType.Int, 9, mc_strCodMarca)

                    .Add(mc_strArroba & mc_strCodModelo, SqlDbType.Int, 9, mc_strCodModelo)


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearSelectCommandOrden2() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelVisita2)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 9, mc_strNoVisita)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsVisita)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Int, 9)
                    .Item(mc_strArroba & mc_strNoVisita).Direction = ParameterDirection.Output

                    .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.VarChar, 50, mc_strNoVehiculo)

                    .Add(mc_strArroba & mc_strIDVehiculo, SqlDbType.Int, 9, mc_strIDVehiculo)

                    .Add(mc_strArroba & mc_strFecha_compromiso, SqlDbType.SmallDateTime, 20, mc_strFecha_compromiso)

                    .Add(mc_strArroba & mc_strAsesor, SqlDbType.Int, 9, mc_strAsesor)

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 50, mc_strCardCode)

                    .Add(mc_strArroba & mc_strCotizacion, SqlDbType.Int, 4, mc_strCotizacion)

                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                'Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdVisita)
                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdVisita)
                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters
                    '''''''''
                    ' .Add(mc_strArroba & mc_strFecha_entrega, SqlDbType.DateTime, 20, mc_strFecha_entrega)
                    ''''''''''
                    'Agregado para actualizar tambien el valor del vehiculo que ahora 
                    'está en la TB_Visitas.  Alejandra 18/05/06

                    .Add(mc_strArroba & mc_strNoVisita, SqlDbType.Decimal, 9, mc_strNoVisita)

                    '.Add(mc_strArroba & mc_strCono, SqlDbType.Int, 4, mc_strCono)

                    '.Add(mc_strArroba & mc_strFecha_compromiso, SqlDbType.DateTime, 20, mc_strFecha_compromiso)

                    .Add(mc_strArroba & mc_strCodEstado, SqlDbType.Int, 4, mc_strCodEstado)

                    '.Add(mc_strArroba & mc_strAsesor, SqlDbType.VarChar, 100, mc_strAsesor)

                    '.Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 50, mc_strCardCode)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            'Try

            '    Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelVehiculo)

            '    cmdIns.CommandType = CommandType.StoredProcedure

            '    With cmdIns.Parameters

            '        .Add(mc_strArroba & mc_strNoVehiculo, SqlDbType.Int, 4, mc_strNoVehiculo)

            '    End With

            '    Return cmdIns

            'Catch ex As Exception

            'End Try

        End Function


#End Region



    End Class

End Namespace

