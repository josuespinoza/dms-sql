Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess


    Public Class RepuestosDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_intCodEstado As String = "CodEstadoRep"
        Private Const mc_intNoRepuesto As String = "NoRepuesto"
        Private Const mc_intNoPiezaPrincipal As String = "NoPiezaPrincipal"
        Private Const mc_intNoSeccion As String = "NoSeccion"

        Private m_adpResp As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_UPDResp As String = "SCGTA_SP_UpdRepuestoXOrden"
        Private Const mc_strSCGTA_SP_SELResp As String = "SCGTA_SP_SELRepuestosXOrden"
        Private Const mc_strSCGTA_SP_DelResp As String = "SCGTA_SP_DELRepuestoXOrden"
        Private Const mc_strSCGTA_SP_InsRep As String = "SCGTA_SP_INSRepuestoXOrden"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpResp = New SqlClient.SqlDataAdapter
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

        Public Overloads Function Fill(ByVal dataSet As RepuestosDataset, ByVal decNoOrden As String, ByVal codestado As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpResp.SelectCommand = CrearSelectCommand()

                m_adpResp.SelectCommand.Parameters(mc_strArroba & mc_strNoOrden).Value = decNoOrden

                m_adpResp.SelectCommand.Parameters(mc_strArroba & mc_intCodEstado).Value = codestado


                m_adpResp.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpResp.Fill(dataSet.SCGTA_TB_RepuestosxOrden)

            Catch ex As Exception
                Throw ex
            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function


#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELResp)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters

                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intCodEstado, SqlDbType.Int, 5, mc_intCodEstado)

                End With

                Return cmdSel

            Catch ex As Exception
                Return Nothing
            End Try


        End Function
        Public Overloads Function Update(ByVal dataSet As RepuestosDataset) As String


            Try
                'm_adpResp.InsertCommand = CreateInsertCommand()
                'm_adpResp.InsertCommand.Connection = m_cnnSCGTaller

                m_adpResp.UpdateCommand = CrearUpdateCommand()
                m_adpResp.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpResp.Update(dataSet.SCGTA_TB_RepuestosxOrden)

                Return ""

            Catch ex As Exception

                MsgBox(ex.Message)
                Return Nothing
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDResp)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters


                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50, mc_strNoOrden)

                    .Add(mc_strArroba & mc_intNoRepuesto, SqlDbType.Int, 4, mc_intNoRepuesto)

                    .Add(mc_strArroba & mc_intNoPiezaPrincipal, SqlDbType.Int, 9, mc_intNoPiezaPrincipal)

                    .Add(mc_strArroba & mc_intNoSeccion, SqlDbType.Int, 5, mc_intNoSeccion)

                    .Add(mc_strArroba & mc_intCodEstado, SqlDbType.Int, 5, mc_intCodEstado)

                End With

                Return cmdUPD

            Catch ex As Exception
                Return Nothing
            End Try

        End Function


#End Region


    End Class

End Namespace