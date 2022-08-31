Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class ConfiguracionDataAdapter
        Implements IDataAdapter
#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strPropiedad As String = "Propiedad"
        Private Const mc_strPropiedad_Orig As String = "Propiedad_Orig"
        Private Const mc_strDescripcion As String = "Descripcion"
        Private Const mc_strValor As String = "Valor"
        Private Const mc_strEtiqueta As String = "Etiqueta"

        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELConfiguracion As String = "SCGTA_SP_SelConfiguracion"
        Private Const mc_strSCGTA_SP_INSConfiguracion As String = "SCGTA_SP_InsConfiguracion"
        Private Const mc_strSCGTA_SP_UpdConfiguracion As String = "SCGTA_SP_UpdConfiguracion"
        Private Const mc_strSCGTA_SP_DelConfiguracion As String = "SCGTA_SP_DelConfiguracion"
        Private Const mc_strSCGTA_SP_SELBodegasXCC As String = "SCGTA_SP_SELBodegasXCC"

        Private m_adpConfiguracion As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        'Modificacion
        Public objDAConexion As DAConexion


#End Region

#Region "Inicializa Configuracion"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpConfiguracion = New SqlClient.SqlDataAdapter

        End Sub

        Public Sub New(ByVal conexion As String)
            Try
                If Not String.IsNullOrEmpty(conexion) Then
                    objDAConexion = New DAConexion
                    objDAConexion.strConectionString = conexion
                End If

                m_cnnSCGTaller = New SqlClient.SqlConnection(conexion)
                m_adpConfiguracion = New SqlClient.SqlDataAdapter
            Catch ex As Exception
                Throw
            End Try
        End Sub


#End Region

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill
            Return Nothing
        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update
            Return Nothing
        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Return Nothing
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Return Nothing
        End Function

        Public Property MissingMappingAction() As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction

            Get
                Return Nothing
            End Get

            Set(ByVal Value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get
                Return Nothing
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


        Public Overloads Function Update(ByVal dataSet As ConfiguracionDataSet) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfiguracion.InsertCommand = CreateInsertCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.InsertCommand.CommandTimeout = 480
                m_adpConfiguracion.InsertCommand.Connection = m_cnnSCGTaller

                m_adpConfiguracion.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.UpdateCommand.CommandTimeout = 480
                m_adpConfiguracion.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpConfiguracion.DeleteCommand = CrearDeleteCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.DeleteCommand.CommandTimeout = 480
                m_adpConfiguracion.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpConfiguracion.Update(dataSet.SCGTA_TB_Configuracion)

            Catch ex As Exception

                Throw
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Update(ByVal datatable As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfiguracion.InsertCommand = CreateInsertCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.InsertCommand.CommandTimeout = 480
                m_adpConfiguracion.InsertCommand.Connection = m_cnnSCGTaller

                m_adpConfiguracion.UpdateCommand = CrearUpdateCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.UpdateCommand.CommandTimeout = 480
                m_adpConfiguracion.UpdateCommand.Connection = m_cnnSCGTaller

                m_adpConfiguracion.DeleteCommand = CrearDeleteCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.DeleteCommand.CommandTimeout = 480
                m_adpConfiguracion.DeleteCommand.Connection = m_cnnSCGTaller

                Call m_adpConfiguracion.Update(datatable)

            Catch ex As Exception

                Throw
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

        Public Overloads Function Fill(ByVal dataSet As ConfiguracionDataSet) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfiguracion.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.SelectCommand.CommandTimeout = 480
                m_adpConfiguracion.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpConfiguracion.Fill(dataSet.SCGTA_TB_Configuracion)

            Catch ex As Exception

                Throw

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Overloads Sub FillBodegasXCC(ByRef p_dstBodegasXCC As ConfBodegasXCentroCostoDataSet)

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfiguracion.SelectCommand = CrearSelectBodegasXCCCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.SelectCommand.CommandTimeout = 480
                m_adpConfiguracion.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpConfiguracion.Fill(p_dstBodegasXCC.SCGTA_SP_SelConfBodegasXCentroCosto)

            Catch ex As Exception

                Throw

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Sub

        Public Overloads Function Fill(ByVal datatble As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpConfiguracion.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpConfiguracion.SelectCommand.CommandTimeout = 480
                m_adpConfiguracion.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpConfiguracion.Fill(datatble)

            Catch ex As Exception

                Throw

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Shared Function DevuelveValorDeParametosConfiguracion(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                     ByVal strPropiedad As String, _
                                                                     ByRef strValor As String) As Boolean

            Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then

                    strValor = drwConfiguracion.Valor
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                throw
            End Try

        End Function


        Public Shared Function DevuelveValorDeParametosConfiguracionValorBooleano(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                     ByVal strPropiedad As String, _
                                                                     ByRef strValor As String) As Boolean

            Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then

                    strValor = drwConfiguracion.Valor

                    If strValor = "1" Then
                        Return True
                    Else
                        Return False
                    End If

                Else
                    Return False
                End If

            Catch ex As Exception
                Throw
            End Try

        End Function

        Public Shared Function DevuelveValorDeParametosConfiguracionUsaCentroCosto(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                     ByVal strPropiedad As String, _
                                                                     ByRef strValor As String) As Boolean

            Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then

                    strValor = drwConfiguracion.Valor

                    If strValor = "1" Then
                        Return True
                    Else
                        Return False
                    End If

                Else
                    Return False
                End If

            Catch ex As Exception
                Throw
            End Try

        End Function

        ''' <summary>
        ''' Retorna el Centro de Beneficio (Norma Reparto SBO) configurado
        ''' para un tipo de orden dado
        ''' </summary>
        ''' <param name="p_intCodTipoOrden">Código del tipo de orden</param>
        ''' <returns>Centro de beneficio asociado</returns>
        ''' <remarks>Si no tiene ningun centro de beneficio (norma de reparto) configurado devuelve
        ''' la cadena vacía</remarks>
        Public Shared Function RetornaCentroBeneficioByTipoOrden(ByVal p_intCodTipoOrden As Integer, ByVal conStr As String) As String
            Dim con As SqlConnection = New SqlConnection(conStr)
            Try
                Dim cmdCB As New SqlClient.SqlCommand("SCGTA_SP_SELCentroBeneficioXTipoOrden", con)
                Dim cb As String

                con.Open()

                With cmdCB
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@codTipoOrden", SqlDbType.Int).Value = p_intCodTipoOrden
                    cb = CType(.ExecuteScalar, String)
                End With

                Return cb
            Catch ex As Exception
                throw
            Finally
                con.Close()
            End Try
            Return String.Empty
        End Function

        Public Shared Function RetornaCentroBeneficioByNoOrden(ByVal p_NoOrden As String, ByVal conStr As String) As String
            Dim con As SqlConnection = New SqlConnection(conStr)
            Try
                Dim cmdCB As New SqlClient.SqlCommand("SCGTA_SP_SELCentroBeneficioXNoOrden", con)
                Dim cb As String

                con.Open()

                With cmdCB
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@noOT", SqlDbType.NVarChar, 50).Value = p_NoOrden
                    cb = CType(.ExecuteScalar, String)
                End With

                Return cb
            Catch ex As Exception
                throw
            Finally
                con.Close()
            End Try
            Return String.Empty
        End Function


        ''' <summary>
        ''' Retorna el Centro de Beneficio (Norma Reparto SBO) configurado
        ''' para un articulo. Se obtiene el Centro de Costo asociado al artículo
        ''' en SBO y si tiene asignado un Centro de Costo se busca el Centro de Beneficio
        ''' configurado para dicho Centro de Costo
        ''' </summary>
        ''' <param name="p_strItemCode">Código del artículo</param>
        ''' <returns>Centro de beneficio asociado</returns>
        ''' <remarks>Si no tiene ningun centro de beneficio (norma de reparto) configurado devuelve
        ''' la cadena vacía</remarks>
        Public Shared Function RetornaCentroBeneficioByItem(ByVal p_strItemCode As String, ByVal conStr As String) As String
            Dim con As SqlConnection = New SqlConnection(conStr)
            Try
                Dim cmdCB As New SqlClient.SqlCommand("SCGTA_SP_SELCentroBeneficioXItem", con)
                Dim cb As String

                con.Open()
                
                With cmdCB
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@ItemCode", SqlDbType.NVarChar, 100).Value = p_strItemCode
                    cb = CType(.ExecuteScalar, String)
                End With

                Return cb
            Catch ex As Exception
                throw
            Finally
                con.Close()
            End Try
            Return String.Empty
        End Function



        Public Shared Sub DevuelveBodegasXCCConfiguracion(ByRef p_dstBodegasXCC As ConfBodegasXCentroCostoDataSet, _
                                    ByVal p_strCentroCosto As String, ByVal p_strTipoItem As String, ByRef p_strBodegaReturn As String)
            Dim intCentroCosto As Integer
            Dim drwBodegasXCC As ConfBodegasXCentroCostoDataSet.SCGTA_SP_SelConfBodegasXCentroCostoRow
            Dim strResult As String = ""

            If Not String.IsNullOrEmpty(p_strCentroCosto) Then
                intCentroCosto = p_strCentroCosto

                drwBodegasXCC = p_dstBodegasXCC.SCGTA_SP_SelConfBodegasXCentroCosto.FindByIDCentroCosto(intCentroCosto)

                If Not drwBodegasXCC Is Nothing Then

                    With drwBodegasXCC
                        Select Case p_strTipoItem
                            Case "BodegaRepuestos"
                                strResult = .Repuestos
                            Case "BodegaSuministros"
                                strResult = .Suministros
                            Case "BodegaServicios"
                                strResult = .Servicios
                            Case "BodegaServiciosExternos"
                                strResult = .ServiciosEX
                            Case "BodegaProceso"
                                strResult = .Proceso
                        End Select
                    End With
                End If

                If Not String.IsNullOrEmpty(strResult) Then
                    p_strBodegaReturn = strResult
                Else
                    p_strBodegaReturn = ""
                End If

            Else
                p_strBodegaReturn = ""
            End If

        End Sub

        Public Shared Function DevuelveEtiquetaDeParametosConfiguracion(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                        ByVal strPropiedad As String, _
                                                                        ByRef strEtiqueta As String) As Boolean

            Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)

                If Not drwConfiguracion Is Nothing _
                   AndAlso Not drwConfiguracion.IsEtiquetaNull Then

                    strEtiqueta = drwConfiguracion.Etiqueta
                    Return True

                Else
                    Return False
                End If

            Catch ex As Exception
                throw
            End Try

        End Function

        Public Shared Function DevuelveValorDeParametosConfiguracionDraft(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                                     ByVal strPropiedad As String, _
                                                                     ByRef strValor As String) As Boolean

            Dim drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            Try

                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)
                strValor = ""
                If Not drwConfiguracion Is Nothing _
                   AndAlso drwConfiguracion.Valor <> "" Then

                    If drwConfiguracion.Valor = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Catch ex As Exception
                throw
            End Try

        End Function


#End Region

#Region "Creación de comandos"



        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdConfiguracion)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strPropiedad, SqlDbType.NVarChar, 50, mc_strPropiedad)

                    'param = .Add(mc_strArroba & mc_strPropiedad_Orig, SqlDbType.NVarChar, 50, mc_strPropiedad)
                    'param.SourceVersion = DataRowVersion.Original

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 200, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strValor, SqlDbType.NVarChar, 500, mc_strValor)

                    .Add(mc_strArroba & mc_strEtiqueta, SqlDbType.NVarChar, 500, mc_strEtiqueta)

                End With

                Return cmdIns

            Catch ex As Exception
                throw
                Return Nothing
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelConfiguracion)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strPropiedad, SqlDbType.NVarChar, 50, mc_strPropiedad)
                End With

                Return cmdIns

            Catch ex As Exception
                throw
                Return Nothing
            End Try

        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSConfiguracion)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strPropiedad, SqlDbType.NVarChar, 50, mc_strPropiedad)

                    .Add(mc_strArroba & mc_strDescripcion, SqlDbType.NVarChar, 200, mc_strDescripcion)

                    .Add(mc_strArroba & mc_strValor, SqlDbType.NVarChar, 500, mc_strValor)

                    .Add(mc_strArroba & mc_strEtiqueta, SqlDbType.NVarChar, 500, mc_strEtiqueta)

                End With

                Return cmdIns

            Catch ex As Exception
                throw
                Return Nothing

            End Try

        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConfiguracion)

                cmdSel.CommandType = CommandType.StoredProcedure

                'With cmdSel.Parameters

                '    'Parametros o criterios de búsqueda 


                'End With

                Return cmdSel

            Catch ex As Exception
                throw
                Return Nothing
            End Try


        End Function

        Private Function CrearSelectBodegasXCCCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELBodegasXCC)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                throw
                Return Nothing
            End Try

        End Function


#End Region

    End Class
End Namespace
