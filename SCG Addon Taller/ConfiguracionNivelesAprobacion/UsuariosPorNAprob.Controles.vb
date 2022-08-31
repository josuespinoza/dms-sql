Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class UsuariosPorNAprob

#Region "Declaraciones"

    
#End Region
    
#Region "Propiedades"

#End Region

#Region "Metodos"
    
    'CARGA EL XML DE LA PANTALLA 
    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function
    
    Private Sub LinkMatriz()

        'datatable que es la matriz de usuarios
        dtUsuariosConsulta = oForm.DataSources.DataTables.Add(strtb_LocalUserConsulta)
        dtUsuariosMatriz = oForm.DataSources.DataTables.Add(strtb_LocalUser)
        dtUsuariosMatriz.Columns.Add("id", BoFieldsType.ft_AlphaNumeric, 100)
        dtUsuariosMatriz.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
        dtUsuariosMatriz.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)

        'Instancia de la matriz de usuarios
        MatrizUsuarios = New MatrizUsuarios(strMatrizUsuarios, oForm, strtb_LocalUser)
        MatrizUsuarios.CreaColumnas()
        MatrizUsuarios.LigaColumnas()

    End Sub


#End Region

End Class
