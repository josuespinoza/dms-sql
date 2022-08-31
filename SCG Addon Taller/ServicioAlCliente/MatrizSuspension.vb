Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSuspension : Inherits MatrixSBO

#Region "New"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Propiedades Columnas"

    Private _ColumnaCol_Fecha As ColumnaMatrixSBOEditText(Of Date)
    Public Property ColumnaCol_Fecha() As ColumnaMatrixSBOEditText(Of Date)
        Get
            Return _ColumnaCol_Fecha
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Date))
            _ColumnaCol_Fecha = value
        End Set
    End Property

    Private _ColumnaCol_Desde As ColumnaMatrixSBOEditText(Of String)
    Public Property ColumnaCol_Desde() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColumnaCol_Desde
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColumnaCol_Desde = value
        End Set
    End Property

    Private _ColumnaCol_Hasta As ColumnaMatrixSBOEditText(Of String)
    Public Property ColumnaCol_Hasta() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColumnaCol_Hasta
        End Get

        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColumnaCol_Hasta = value
        End Set
    End Property


#End Region


    Public Overrides Sub CreaColumnas()
        _ColumnaCol_Fecha = New ColumnaMatrixSBOEditText(Of Date)("Col_Fecha", True, "fhaSusp", Me)
        _ColumnaCol_Desde = New ColumnaMatrixSBOEditText(Of String)("Col_Desde", True, "HraDesde", Me)
        _ColumnaCol_Hasta = New ColumnaMatrixSBOEditText(Of String)("Col_Hasta", True, "HraHasta", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _ColumnaCol_Fecha.AsignaBindingDataTable()
        _ColumnaCol_Desde.AsignaBindingDataTable()
        _ColumnaCol_Hasta.AsignaBindingDataTable()
    End Sub
End Class
