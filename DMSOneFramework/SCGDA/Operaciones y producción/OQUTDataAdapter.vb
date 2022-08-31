Imports DMSOneFramework.SCGDataAccess.DAConexion

Namespace SCGDataAccess
    Public Class OQUTDataAdapter
        Implements IDataAdapter

#Region "Declaraciones"

        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strOTPadre As String = "OTPadre"

        Private Const mc_intTipoOrden As String = "CodTipoOrden"

        Private Const mc_strOrden As String = "NoOrden"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"

        Private m_adpOrden As SqlClient.SqlDataAdapter

        Private Const mc_strSCGTA_SP_SelOQUT As String = "SCGTA_SP_SelOQUT"

        Private m_cnnSCGTaller As SqlClient.SqlConnection

        Private Const mc_strArroba As String = "@"
        Private objDAConexion As DAConexion

#End Region

#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

            m_adpOrden = New SqlClient.SqlDataAdapter
        End Sub

        Public Sub New(ByVal strCadenaConexion As String)

            m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

            m_adpOrden = New SqlClient.SqlDataAdapter

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
                Throw New NotImplementedException()
            End Get
        End Property

#End Region

#Region "Implementaciones SCG"

        Public Overloads Function Fill(ByVal dataSet As OrdenEspecialDataset, _
                                       Optional ByVal p_intNoCotizacion As Integer = -1, _
                                       Optional ByVal p_strOTPadre As String = "") As Integer

            Try
                m_adpOrden.SelectCommand = CrearSelectCommand()
                'Erick Sanabria Bravo. 05.11.2013 Se modifica Command TimeOut a 480.
                m_adpOrden.SelectCommand.CommandTimeout = 480

                If m_cnnSCGTaller.State = ConnectionState.Closed Then
                    If m_cnnSCGTaller.ConnectionString = "" Then
                        m_cnnSCGTaller.ConnectionString = strConexionADO
                    End If
                    m_cnnSCGTaller.Open()
                End If

                If p_intNoCotizacion <> -1 Then
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strNoCotizacion).Value = p_intNoCotizacion
                End If

                If p_strOTPadre <> "" Then
                    m_adpOrden.SelectCommand.Parameters(mc_strArroba & mc_strOTPadre).Value = p_strOTPadre
                End If

                m_adpOrden.SelectCommand.Connection = m_cnnSCGTaller

                Call m_adpOrden.Fill(dataSet.SCGTA_TB_Orden)

            Catch ex As Exception
                Throw ex
            Finally
                m_cnnSCGTaller.Close()
            End Try

        End Function

#End Region

#Region "Creación de comandos"

        Private Function CrearSelectCommand() As SqlClient.SqlCommand

            Try

                Dim cmdSel As New SqlClient.SqlCommand(mc_strSCGTA_SP_SelOQUT)

                cmdSel.CommandType = CommandType.StoredProcedure

                With cmdSel.Parameters
                    'Parametros o criterios de búsqueda 

                    .Add(mc_strArroba & mc_strNoCotizacion, SqlDbType.Int, 4, mc_strNoCotizacion)
                    .Add(mc_strArroba & mc_strOTPadre, SqlDbType.NVarChar, 20, mc_strOTPadre)

                End With

                Return cmdSel

            Catch ex As Exception
                Throw ex
            End Try


        End Function

#End Region

    End Class
End Namespace