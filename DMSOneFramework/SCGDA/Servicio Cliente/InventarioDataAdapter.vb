Option Strict On
Option Explicit On 
Namespace SCGDataAccess


    Public Class InventarioDataAdapter

        Implements IDataAdapter


#Region "Declaraciones"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strNoExpediente As String = "NoExpediente"
        Private Const mc_strCodDetalle As String = "CodDetalle"
        Private Const mc_strDetalle As String = "Detalle"
       
        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_UpdInventario As String = "SCGTA_SP_UpdInventario"
        Private Const mc_strSCGTA_SP_SELInventario As String = "SCGTA_SP_SELInventario"
        Private Const mc_strSCGTA_SP_InsInventario As String = "SCGTA_SP_InsInventario"
        Private Const mc_strSCGTA_SP_DelInventario As String = "SCGTA_SP_DelInventario"


        Private m_adpInventario As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"

        Dim objDAConexion As DAConexion

#End Region


#Region "Inicializa InventarioDataAdapter"

        Public Sub New()

            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion
            m_adpInventario = New SqlClient.SqlDataAdapter

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


        Public Overloads Function Fill(ByVal dataSet As InventarioDataset, ByVal decNoExtpediente As Decimal) As Integer

            Try

                'Call m_cnnSCGTaller.Open()
                
                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpInventario.SelectCommand = CrearSelectCommandByNoExpediente()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado
                If decNoExtpediente = Nothing Then
                    m_adpInventario.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = System.DBNull.Value
                Else
                    m_adpInventario.SelectCommand.Parameters(mc_strArroba & mc_strNoExpediente).Value = decNoExtpediente
                End If

                m_adpInventario.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpInventario.Fill(dataSet.SCGTA_TB_Inventario)


            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Overloads Function Insert(ByVal dataSet As InventarioDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpInventario.InsertCommand = CreateInsertCommand()
                m_adpInventario.InsertCommand.Connection = m_cnnSCGTaller

                Call m_adpInventario.Update(dataSet.SCGTA_TB_Inventario)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()


            End Try

        End Function


        Public Function Delete(ByVal dataset As InventarioDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpInventario.DeleteCommand = CrearDeleteCommand()

                m_adpInventario.DeleteCommand.Connection = m_cnnSCGTaller

                'm_adpInventario.DeleteCommand.Parameters(mc_strArroba & mc_strCodDetalle).SourceColumn = dataset.SCGTA_TB_Inventario.CodDetalleColumn.ColumnName

                m_adpInventario.Update(dataset.SCGTA_TB_Inventario)

            Catch ex As Exception

                Throw ex
            Finally
                m_cnnSCGTaller.Close()

            End Try
        End Function



#End Region



#Region "Creación de comandos"


        Private Function CrearSelectCommandByNoExpediente() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELInventario)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Decimal, 9, mc_strNoExpediente)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsInventario)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters

                    .Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int, 9, mc_strNoExpediente)

                    .Add(mc_strArroba & mc_strDetalle, SqlDbType.VarChar, 500, mc_strDetalle)

                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function CrearDeleteCommand() As SqlClient.SqlCommand

            Try

                Dim cmdDel As New SqlClient.SqlCommand(mc_strSCGTA_SP_DelInventario)

                cmdDel.CommandType = CommandType.StoredProcedure

                With cmdDel.Parameters

                    .Add(mc_strArroba & mc_strCodDetalle, SqlDbType.Decimal, 9, mc_strCodDetalle)

                End With

                Return cmdDel

            Catch ex As Exception
                Throw ex
            End Try

        End Function


#End Region


    End Class

End Namespace