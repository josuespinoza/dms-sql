Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.BLSBO
Imports System.Data.SqlClient

Namespace SCGDataAccess
    Public Class ClsRepuestosSBO

#Region "Declaraciones"

        Private Shared m_cnnSCGTaller As SqlClient.SqlConnection

        Dim objDAConexion As DAConexion


#End Region


#Region "Constructor"

        Public Sub New()
            objDAConexion = New DAConexion
            m_cnnSCGTaller = objDAConexion.ObtieneConexion

        End Sub


#End Region

        Public Function agregarRepuesto(ByVal p_CodRepuesto As String, ByVal p_DescripRepuesto As String _
                                       , ByVal COMPANIA As String, ByVal strDATABASESCG As String) As Long

            Dim oRepuesto As SAPbobsCOM.Items

            Dim strError As String =  String.Empty
            Dim lngError As Long
            Dim lngResultado As Long



            oRepuesto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            If (oRepuesto.GetByKey(p_CodRepuesto) = False) Then 'Inserta solo si no existe un repuesto con ese código
                'Agrega los datos
                oRepuesto.ItemCode = p_CodRepuesto
                oRepuesto.ItemName = p_DescripRepuesto
                oRepuesto.ItemsGroupCode = Configuracion.DevuelveValordeParametro("'GrupoRepuestos'", COMPANIA, strDATABASESCG, objBLConexion)
                oRepuesto.GLMethod = SAPbobsCOM.BoGLMethods.glm_ItemClass
                oRepuesto.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES
                oRepuesto.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES
                oRepuesto.DefaultWarehouse = Configuracion.DevuelveValordeParametro("'Bodega'", COMPANIA, strDATABASESCG, objBLConexion)

                lngResultado = oRepuesto.Add()

                If (lngResultado <> 0) Then
                    oCompany.GetLastError(lngError, strError)
                    Throw New SCGCommon.ExceptionsSBO(lngError, strError)
                    'MsgBox("Error:" + Str(lngError) + "," + strError)
                    lngResultado = lngError
                End If

            Else
                lngResultado = -1
            End If

            Return lngResultado
        End Function

    End Class
End Namespace