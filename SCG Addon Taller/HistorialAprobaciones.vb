
Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM

Partial Public Class HistorialAprobaciones : Implements IFormularioSBO


#Region "... Declaraciones ..."

    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    Public n As NumberFormatInfo

    Private g_dtLocal As SAPbouiCOM.DataTable
    Private Const g_strDtConsul As String = "dtConsul"
    Private strAntTipoOT As String

#End Region

#Region "... Constructor ..."

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal p_SBOAplication As SAPbouiCOM.Application)

        m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
        m_oCompany = ocompany
        m_SBO_Application = p_SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "... Propiedades ..."

#End Region

#Region "... Inicializacion de Controles ..."

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

            userDS.Add("noOT", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("noCot", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("idSuc", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("chkSelAll", BoDataType.dt_SHORT_TEXT, 1)

            CargaFormulario()
        End If

    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        'Manejo de formulario
        FormularioSBO.Freeze(True)

        'Manejo de formulario
        FormularioSBO.Freeze(False)
    End Sub

    Public Sub LoadMatrixLines(ByVal FormUID As String, ByVal pDocEntry As String)

        Dim oForm As SAPbouiCOM.Form
        Dim dtHistorial As DataTable
        Dim query As String
        Dim m_strConsulta As String = "select us.U_NAME Usuario, ad.U_Name U_Nivel, U_Fecha ,	U_Hora,	U_Comentario	from [@SCGD_HIST_CV] hcv		Left join OUSR us on hcv.U_Usuario  = us.USER_CODE		Left join [@SCGD_ADMIN9] ad on hcv.U_Niv_Code = ad.U_Codigo	Where hcv.DocEntry='{0}' Order by U_Fecha, U_Hora"
        Try
            oForm = m_SBO_Application.Forms.Item(g_strFormcomentariosApr)
            oForm.Freeze(True)

            dtHistorial = oForm.DataSources.DataTables.Item(strDataTableLineas)

            query = String.Format(m_strConsulta, pDocEntry)

            dtHistorial.ExecuteQuery(query)

            g_oMtxHist = DirectCast(oForm.Items.Item(mc_strMatrizHist).Specific, Matrix)

            g_oMtxHist.LoadFromDataSource()
            g_oMtxHist.Columns.Item("Col_Com").Width = 500
            g_oMtxHist.Columns.Item("Col_Usr").Width = 200

            oForm.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

End Class

