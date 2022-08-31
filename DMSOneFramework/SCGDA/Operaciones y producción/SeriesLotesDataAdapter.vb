Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess

    Public Class SeriesLotesDataAdapter
        Implements IDataAdapter

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private m_adpSeriesLotes As SqlClient.SqlDataAdapter



        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpSeriesLotes = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

            m_adpSeriesLotes = New SqlClient.SqlDataAdapter

        End Sub


#End Region

        Public Function Fill(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Fill

        End Function

        Public Function FillSchema(ByVal dataSet As System.Data.DataSet, ByVal schemaType As System.Data.SchemaType) As System.Data.DataTable() Implements System.Data.IDataAdapter.FillSchema

        End Function

        Public Function GetFillParameters() As System.Data.IDataParameter() Implements System.Data.IDataAdapter.GetFillParameters

        End Function

        Public Property MissingMappingAction() As System.Data.MissingMappingAction Implements System.Data.IDataAdapter.MissingMappingAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingMappingAction)

            End Set
        End Property

        Public Property MissingSchemaAction() As System.Data.MissingSchemaAction Implements System.Data.IDataAdapter.MissingSchemaAction
            Get

            End Get
            Set(ByVal value As System.Data.MissingSchemaAction)

            End Set
        End Property

        Public ReadOnly Property TableMappings() As System.Data.ITableMappingCollection Implements System.Data.IDataAdapter.TableMappings
            Get

            End Get
        End Property

        Public Function Update(ByVal dataSet As System.Data.DataSet) As Integer Implements System.Data.IDataAdapter.Update

        End Function

        Public Overloads Function Fill_SeriesLotes(ByVal dataSet As SeriesLotesDataSet, ByVal p_baseType As Integer, ByVal p_baseDocEntry As Integer, ByVal p_isInvntItem As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If



                m_adpSeriesLotes.SelectCommand = Me.CrearSelectCommandSeriesLotes(p_baseType, p_baseDocEntry, p_isInvntItem)
                m_adpSeriesLotes.SelectCommand.Connection = m_cnnSCGTaller

                Fill_SeriesLotes = m_adpSeriesLotes.Fill(dataSet.SeriesLotes)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CrearSelectCommandSeriesLotes(ByVal p_baseType As Integer, ByVal p_baseDocEntry As Integer, ByVal p_IsInventariable As String) As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand("SCGTA_SP_SELSeriesLotes")

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & "baseType", SqlDbType.Int, 80).Value = p_baseType
                    .Add(mc_strArroba & "isInvntItem", SqlDbType.VarChar, 30).Value = p_IsInventariable
                    .Add(mc_strArroba & "baseDocEntry", SqlDbType.Int, 80).Value = p_baseDocEntry

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Overloads Function Fill_Transferencias(ByVal dataSet As TransferenciasPorCotizacionDataSet, ByVal p_CodCotizacion As Integer) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If



                m_adpSeriesLotes.SelectCommand = Me.CrearSelectCommandTransferencias(p_CodCotizacion)
                m_adpSeriesLotes.SelectCommand.Connection = m_cnnSCGTaller

                Fill_Transferencias = m_adpSeriesLotes.Fill(dataSet.TransferenciasPorCotizacion)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CrearSelectCommandTransferencias(ByVal p_CodCotizacion As Integer) As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand("SCGTA_SP_SELTransferencias")

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & "CodCotizacion", SqlDbType.Int, 80).Value = p_CodCotizacion


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Overloads Function Fill_CotizacionPorOTPadre(ByVal dataSet As TransferenciasPorCotizacionDataSet, ByVal p_OTPadre As String) As Integer

            Try

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    Call m_cnnSCGTaller.Open()
                End If



                m_adpSeriesLotes.SelectCommand = Me.CrearSelectCommandCotizacionPorOTPadre(p_OTPadre)
                m_adpSeriesLotes.SelectCommand.Connection = m_cnnSCGTaller

                Fill_CotizacionPorOTPadre = m_adpSeriesLotes.Fill(dataSet.CotizacionPorOTPadre)


            Catch ex As Exception
                Throw ex
            Finally
                Call m_cnnSCGTaller.Close()
            End Try

        End Function

        Private Function CrearSelectCommandCotizacionPorOTPadre(ByVal p_OTPadre As String) As SqlClient.SqlCommand
            Try

                Dim cmdSel As New SqlClient.SqlCommand("SCGTA_SP_SELCotizacionPorOTPadre")

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & "OTPadre", SqlDbType.VarChar, 80).Value = p_OTPadre


                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class
End Namespace

