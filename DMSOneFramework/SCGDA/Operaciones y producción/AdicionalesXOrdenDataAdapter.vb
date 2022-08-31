Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Namespace SCGDataAccess

    Public Class AdicionalesXOrdenDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

#Region "Constantes"

        'Declaración de las constantes con el nombre de las columnas del Dataset.
        Private Const mc_strID As String = "ID"
        Private Const mc_strNoAdicional As String = "NoAdicional"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strMontoMO As String = "MontoMO"
        Private Const mc_strCantidadRep As String = "CantidadRep"
        Private Const mc_strCantidadSum As String = "CantidadSum"
        Private Const mc_strFecha As String = "Fecha"
        'Agregado 22/06/06. Alejandra
        Private Const mc_strMontoSum As String = "MontoSum"


        'Declaracion de las constantes con el nombre de los procedimientos almacenados
        Private Const mc_strSCGTA_SP_SELAdicionalesXOrden As String = "SCGTA_SP_SELAdicionalesXOrden"
        Private Const mc_strSCGTA_SP_SELAdicionalesXOrdenByNoOrden As String = "SCGTA_SP_SELAdicionalesXOrdenByNoOrden"
        Private Const mc_strSCGTA_SP_INSAdicionalesXOrden As String = "SCGTA_SP_INSAdicionalesXOrden"
        Private Const mc_strSCGTA_SP_UPDAdicionalesXOrden As String = "SCGTA_SP_UPDAdicionalesXOrden"

        'Nombre de la tabla de adicionalesXorden
        Private Const mc_strTableName As String = "SCGTA_TB_AdicionalesXOrden"


        Private Const mc_strArroba As String = "@"

#End Region

#Region "Variables"

        Private m_adpAdicionalesXOrden As SqlClient.SqlDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion

#End Region

#End Region

#Region "Inicializa DataAdapter"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpAdicionalesXOrden = New SqlClient.SqlDataAdapter
        End Sub


#End Region

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

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal p_dstAxO As AdicionalesXOrdenDataset) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAdicionalesXOrden.SelectCommand = CrearSelectCommand()
                m_adpAdicionalesXOrden.SelectCommand.Connection = m_cnnSCGTaller

                m_adpAdicionalesXOrden.Fill(p_dstAxO.SCGTA_TB_AdicionalesXOrden)

            Catch ex As Exception

                Throw ex

            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

        Public Overloads Function Fill(ByVal p_dstAxO As AdicionalesXOrdenDataset, ByVal p_strNoORden As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAdicionalesXOrden.SelectCommand = CrearSelectCommandByNoOrden()
                m_adpAdicionalesXOrden.SelectCommand.Connection = m_cnnSCGTaller

                With m_adpAdicionalesXOrden.SelectCommand
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoORden
                End With

                m_adpAdicionalesXOrden.Fill(p_dstAxO.SCGTA_TB_AdicionalesXOrden)

            Catch ex As Exception

                Throw ex

            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

        Public Overloads Function Update(ByVal p_dstAxO As AdicionalesXOrdenDataset) As Integer
            Dim intResult As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                m_adpAdicionalesXOrden.UpdateCommand = CrearUpdateCommand()
                m_adpAdicionalesXOrden.UpdateCommand.Connection = m_cnnSCGTaller

                intResult = m_adpAdicionalesXOrden.Update(p_dstAxO.SCGTA_TB_AdicionalesXOrden)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Function

        Public Sub InsertNewAdicionalbyCMD(ByVal p_strNoOrden As String, ByVal p_intNoAdicional As Integer, _
                                            ByVal p_intCantidadRep As Decimal, ByVal p_dtFecha As Date)
            Dim cmdInsertAxO As SqlClient.SqlCommand

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If

                cmdInsertAxO = CrearInsertCommand()

                cmdInsertAxO.Connection = m_cnnSCGTaller

                With cmdInsertAxO
                    .Parameters(mc_strArroba & mc_strNoOrden).Value = p_strNoOrden
                    .Parameters(mc_strArroba & mc_strNoAdicional).Value = p_intNoAdicional
                    .Parameters(mc_strArroba & mc_strCantidadRep).Value = p_intCantidadRep
                    .Parameters(mc_strArroba & mc_strFecha).Value = p_dtFecha
                End With

                cmdInsertAxO.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally

                If Not IsNothing(m_cnnSCGTaller) Then
                    If m_cnnSCGTaller.State = ConnectionState.Open Then
                        m_cnnSCGTaller.Close()
                    End If
                End If

            End Try

        End Sub

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELAdicionalesXOrden)

            cmdSel.CommandType = CommandType.StoredProcedure

            Return cmdSel

        End Function

        Private Function CrearSelectCommandByNoOrden() As SqlClient.SqlCommand

            Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SELAdicionalesXOrdenByNoOrden)

            cmdSel.CommandType = CommandType.StoredProcedure

            With cmdSel
                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
            End With

            Return cmdSel

        End Function

        Private Function CrearInsertCommand() As SqlClient.SqlCommand

            Try

                Dim cmdIns As New SqlClient.SqlCommand(mc_strSCGTA_SP_INSAdicionalesXOrden)

                cmdIns.CommandType = CommandType.StoredProcedure

                With cmdIns.Parameters
                    .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                    .Add(mc_strArroba & mc_strNoAdicional, SqlDbType.Int, 4)
                    .Add(mc_strArroba & mc_strCantidadRep, SqlDbType.Decimal, 9)
                    .Add(mc_strArroba & mc_strFecha, SqlDbType.DateTime, 8)
                End With

                Return cmdIns

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Private Function CrearUpdateCommand() As SqlClient.SqlCommand

            Try

                Dim cmdUpd As New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDAdicionalesXOrden)

                cmdUpd.CommandType = CommandType.StoredProcedure

                With cmdUpd.Parameters
                    .Add(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID)
                    .Add(mc_strArroba & mc_strMontoMO, SqlDbType.Decimal, 9, mc_strMontoMO)
                    .Add(mc_strArroba & mc_strCantidadRep, SqlDbType.Decimal, 9, mc_strCantidadRep)
                    .Add(mc_strArroba & mc_strCantidadSum, SqlDbType.Decimal, 9, mc_strCantidadSum)
                    .Add(mc_strArroba & mc_strMontoSum, SqlDbType.Decimal, 9, mc_strMontoSum)

                End With

                Return cmdUpd

            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class

End Namespace