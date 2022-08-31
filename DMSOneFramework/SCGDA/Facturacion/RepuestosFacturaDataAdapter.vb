Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess
    Public Class RepuestosFacturaDataAdapter
        Implements IDataAdapter

#Region "Implementaciones"

        Private Overloads Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Private Overloads Function Update(ByVal dataset As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema
            Throw New NotImplementedException()
        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters
            Throw New NotImplementedException()
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
                Throw New NotImplementedException()
            End Get
        End Property

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpRFactura = New SqlClient.SqlDataAdapter
        End Sub


#End Region


#Region "Variables"

        Private m_adpRFactura As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#Region "Constantes"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_intdocnum As String = "docnum"
        Private Const mc_intdoctotal As String = "doctotal"
        Private Const mc_strnoorden As String = "noorden"
        Private Const mc_strestado As String = "est_fac"
        Private Const mc_strArroba As String = "@"
        Private Const mc_strFacturada As String = "Facturada"
        Private Const mc_strCadenaFacturas As String = "cadena_facturas"

       

        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SelRepuestosFactura As String = "SCGTA_SP_selRepuestosFacturacion"
        Private Const mc_strSCGTA_SP_SelMontoIns As String = "SCGTA_SP_SELORDEN"
        Private Const mc_strSCGTA_SP_UpdRepuestos As String = "SCGTA_SP_UPDFactura_Repuestos"
        

#End Region

#Region "Implementaciones"

        Public Overloads Function Fill(ByVal dataSet As RepuestoFacturaDataset, ByVal ORDEN As String, ByVal Factur As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpRFactura.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If ORDEN = "" Then
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = System.DBNull.Value
                Else
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = ORDEN
                End If

                If Factur = "" Then
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strFacturada).Value = System.DBNull.Value
                Else
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strFacturada).Value = Factur
                End If

                m_adpRFactura.SelectCommand.Connection = m_cnnSCGTaller

                dataSet.SCGTA_SP_RepuestosFacturacion.CheckColumn.DefaultValue = 0


                Call m_adpRFactura.Fill(dataSet.SCGTA_SP_RepuestosFacturacion)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function

        Public Function Actualizar(ByVal CadenaFacturas As String) As Integer

            'Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRFactura.UpdateCommand = UpdateCommandRepuestos()

                If CadenaFacturas = "" Then
                    m_adpRFactura.UpdateCommand.Parameters(mc_strArroba & mc_strCadenaFacturas).Value = System.DBNull.Value
                Else
                    m_adpRFactura.UpdateCommand.Parameters(mc_strArroba & mc_strCadenaFacturas).Value = CadenaFacturas
                End If


                m_adpRFactura.UpdateCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpRFactura.UpdateCommand.ExecuteReader

                If RNombre.Read Then
                    Return 1
                Else
                    Return 0
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try
        End Function


#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelRepuestosFactura)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel.Parameters

                'Parametros o criterios de búsqueda 

                .Add(mc_strArroba & mc_strnoorden, SqlDbType.VarChar, 50, mc_strnoorden)

                .Add(mc_strArroba & mc_strFacturada, SqlDbType.VarChar, 50, mc_strFacturada)


            End With

            Return cmdSel

        End Function

        Public Function DevuelveMonto(ByVal p_strnoorden As String) As String
            'Dim nombre As String
            Dim RNombre As SqlClient.SqlDataReader

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                m_adpRFactura.SelectCommand = CrearSelectCommandMonto()

                If p_strnoorden = "" Then
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = System.DBNull.Value
                Else
                    m_adpRFactura.SelectCommand.Parameters(mc_strArroba & mc_strnoorden).Value = p_strnoorden
                End If


                m_adpRFactura.SelectCommand.Connection = m_cnnSCGTaller
                RNombre = m_adpRFactura.SelectCommand.ExecuteReader

                If RNombre.Read Then
                    If IsDBNull(RNombre("MontoRepuestos")) Then
                        Return 0
                    Else
                        Return RNombre("MontoRepuestos")
                    End If

                Else
                    Return ""
                End If


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function
        Private Function UpdateCommandRepuestos() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdRepuestos)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCadenaFacturas, SqlDbType.VarChar, 250, mc_strCadenaFacturas)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearSelectCommandMonto() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelMontoIns)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strnoorden, SqlDbType.VarChar, 50, mc_strnoorden)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function
#End Region

    End Class
End Namespace
