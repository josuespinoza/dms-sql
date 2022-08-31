Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class ConfiguraFacturaDataAdapter
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

            m_adpFactura = New SqlClient.SqlDataAdapter
        End Sub


#End Region

#Region "Variables"

        Private m_adpFactura As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#Region "Constantes"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strcodmanoobra As String = "itemcodemanoobra"
        Private Const mc_strcodrepuesto As String = "itemcoderepuestos"
        Private Const mc_strcodsuministro As String = "itemcodesuministros"
        Private Const mc_intimpmanoobra As String = "impmanoobra"
        Private Const mc_intimprepuestos As String = "imprepuestos"
        Private Const mc_intimpsuministro As String = "impsuministros"

        Private Const mc_intconsecutivo As String = "consecutivo"

        Private Const mc_strCodMO As String = "ImpCodigoMO"
        Private Const mc_strCodSum As String = "ImpCodigoSum"
        Private Const mc_strCodRep As String = "ImpCodigoRep"
        Private Const mc_strDescMO As String = "DscriptionMO"
        Private Const mc_strDescSum As String = "DscriptionSum"
        Private Const mc_strDescRep As String = "DscriptionRep"


        Private Const mc_strArroba As String = "@"

        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_InsConfigFactura As String = "SCGTA_SP_INSConfiguraFactura"

        Private Const mc_strSCGTA_SP_UpdConfigFactura As String = "SCGTA_SP_UpdConfiguraFactura"
        Private Const mc_strSCGTA_SP_SELConfFactura As String = "SCGTA_SP_SELConfFactura"


#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByRef dataSet As ConfiguraFacturaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                'Creacion del comando
                m_adpFactura.SelectCommand = CrearSelectCommand()

                '-------------------------------------Se cargan los parámetros----------------------------------------

                'Si el parametro viene vacio se carga como un System.DBNull.Value, osea un valor nulo para SQL Server 
                'sino se asigna el valor encontrado


                m_adpFactura.SelectCommand.Connection = m_cnnSCGTaller



                Call m_adpFactura.Fill(dataSet.SCGTA_TB_ConfFacturacion)

            Catch ex As Exception

                Throw ex

            Finally

                Call m_cnnSCGTaller.Close()

            End Try

        End Function
        Public Overloads Function Update(ByVal dataSet As ConfiguraFacturaDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpFactura.InsertCommand = CreateInsertCommand()
                m_adpFactura.InsertCommand.Connection = m_cnnSCGTaller

                m_adpFactura.UpdateCommand = CrearUpdateCommand()
                m_adpFactura.UpdateCommand.Connection = m_cnnSCGTaller

                Call m_adpFactura.Update(dataSet.SCGTA_TB_ConfFacturacion)

            Catch ex As Exception

                Throw ex

            Finally
                m_cnnSCGTaller.Close()

            End Try

        End Function
#End Region

#Region "Creación de comandos"
        Private Function CreateInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_InsConfigFactura)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters


                    .Add(mc_strArroba & mc_strcodmanoobra, SqlDbType.NVarChar, 20, mc_strcodmanoobra)

                    .Add(mc_strArroba & mc_strcodrepuesto, SqlDbType.NVarChar, 20, mc_strcodrepuesto)

                    .Add(mc_strArroba & mc_strcodsuministro, SqlDbType.NVarChar, 20, mc_strcodsuministro)

                    .Add(mc_strArroba & mc_intimpmanoobra, SqlDbType.Int, 9, mc_intimpmanoobra)

                    .Add(mc_strArroba & mc_intimprepuestos, SqlDbType.Int, 9, mc_intimprepuestos)

                    .Add(mc_strArroba & mc_intimpsuministro, SqlDbType.Int, 9, mc_intimpsuministro)

                    .Add(mc_strArroba & mc_strCodMO, SqlDbType.NVarChar, 8, mc_strCodMO)

                    .Add(mc_strArroba & mc_strCodSum, SqlDbType.NVarChar, 8, mc_strCodSum)

                    .Add(mc_strArroba & mc_strCodRep, SqlDbType.NVarChar, 8, mc_strCodRep)

                    .Add(mc_strArroba & mc_strDescMO, SqlDbType.NVarChar, 100, mc_strDescMO)

                    .Add(mc_strArroba & mc_strDescSum, SqlDbType.NVarChar, 100, mc_strDescSum)

                    .Add(mc_strArroba & mc_strDescRep, SqlDbType.NVarChar, 100, mc_strDescRep)


                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Private Function CrearUpdateCommand()

            Try

                Dim cmdUPD As New SqlClient.SqlCommand(mc_strSCGTA_SP_UpdConfigFactura)

                cmdUPD.CommandType = CommandType.StoredProcedure

                With cmdUPD.Parameters

                    .Add(mc_strArroba & mc_strcodmanoobra, SqlDbType.NVarChar, 20, mc_strcodmanoobra)

                    .Add(mc_strArroba & mc_strcodrepuesto, SqlDbType.NVarChar, 20, mc_strcodrepuesto)

                    .Add(mc_strArroba & mc_strcodsuministro, SqlDbType.NVarChar, 20, mc_strcodsuministro)

                    .Add(mc_strArroba & mc_intimpmanoobra, SqlDbType.Int, 9, mc_intimpmanoobra)

                    .Add(mc_strArroba & mc_intimprepuestos, SqlDbType.Int, 9, mc_intimprepuestos)

                    .Add(mc_strArroba & mc_intimpsuministro, SqlDbType.Int, 9, mc_intimpsuministro)

                    .Add(mc_strArroba & mc_intconsecutivo, SqlDbType.Int, 9, mc_intconsecutivo)

                    .Add(mc_strArroba & mc_strCodMO, SqlDbType.NVarChar, 8, mc_strCodMO)

                    .Add(mc_strArroba & mc_strCodSum, SqlDbType.NVarChar, 8, mc_strCodSum)

                    .Add(mc_strArroba & mc_strCodRep, SqlDbType.NVarChar, 8, mc_strCodRep)

                    .Add(mc_strArroba & mc_strDescMO, SqlDbType.NVarChar, 100, mc_strDescMO)

                    .Add(mc_strArroba & mc_strDescSum, SqlDbType.NVarChar, 100, mc_strDescSum)

                    .Add(mc_strArroba & mc_strDescRep, SqlDbType.NVarChar, 100, mc_strDescRep)


                End With

                Return cmdUPD

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConfFactura)

                cmdSel.CommandType = CommandType.StoredProcedure

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function


#End Region
    End Class
End Namespace
