Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixAvaluos : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Obs As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Check As ColumnaMatrixSBOCheckBox(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaCol_Code As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Code
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Code = value
        End Set
    End Property

    Public Property ColumnaCol_Obs As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Obs
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Obs = value
        End Set
    End Property

    Public Property ColumnaCol_Des As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    Public Property ColumnaCol_Check As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Check
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Check = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

       Public Overrides Sub CreaColumnas()
        ColumnaCol_Code = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "code", Me)
        ColumnaCol_Des = New ColumnaMatrixSBOEditText(Of String)("ColDes", True, "desc", Me)
        ColumnaCol_Check = New ColumnaMatrixSBOCheckBox(Of String)("ColChk", True, "chk", Me)
        ColumnaCol_Obs = New ColumnaMatrixSBOEditText(Of String)("ColObs", True, "obs", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaCol_Code.AsignaBindingDataTable()
        ColumnaCol_Des.AsignaBindingDataTable()
        ColumnaCol_Check.AsignaBindingDataTable()
        ColumnaCol_Obs.AsignaBindingDataTable()
    End Sub

#End Region


End Class
