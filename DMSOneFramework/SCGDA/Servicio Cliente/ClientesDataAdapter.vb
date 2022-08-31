
Option Strict On
Option Explicit On 

Namespace SCGDataAccess

    Public Class ClientesDataAdapter


        Implements IDataAdapter


#Region "Declaraciones"


        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strCardName As String = "CardName"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strPhone1 As String = "Phone1"
        Private Const mc_strPhone2 As String = "Phone2"
        Private Const mc_strTipoCliente As String = "U_tipoCtl"
        Private Const mc_strEmail As String = "E_Mail"



        'Declaración de las variables que determinan el tipo de busqueda..con like % o sin %
        Private mc_strIfNoChasis As String = "IfNoChasis"
        Private mc_strIfNoMotor As String = "IfNoMotor"
        Private mc_strIfPlaca As String = "IfPlaca"


        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELClientes As String = "SCGTA_SP_SELClientes"

        Private m_adpCliente As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion


#End Region


#Region "Inicializa ClientesDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpCliente = New SqlClient.SqlDataAdapter

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


        Public Overloads Function Fill(ByVal dataSet As ClienteDataset, ByVal CardCode As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpCliente.SelectCommand = CrearSelectCommandByCardCode()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado

                m_adpCliente.SelectCommand.Parameters(mc_strArroba & mc_strCardCode).Value = CardCode

                m_adpCliente.SelectCommand.Connection = m_cnnSCGTaller

                dataSet.SCGTA_VW_Clientes.Phone1Column.AllowDBNull = True
                dataSet.SCGTA_VW_Clientes.Phone2Column.AllowDBNull = True
                dataSet.SCGTA_VW_Clientes.NotesColumn.AllowDBNull = True
                dataSet.SCGTA_VW_Clientes.FaxColumn.AllowDBNull = True
                dataSet.SCGTA_VW_Clientes.E_MailColumn.AllowDBNull = True

                Call m_adpCliente.Fill(dataSet.SCGTA_VW_Clientes)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function





#End Region


#Region "Creación de comandos"


        Private Function CrearSelectCommandByCardCode() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELClientes)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strCardCode, SqlDbType.NVarChar, 50, mc_strCardCode)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


#End Region





    End Class

End Namespace