'matriz mensajeria

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizMensajeria
    : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Sucu As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_CSucu As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_CNAp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_LineId As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaColName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Name
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Name = value
        End Set
    End Property

    Public Property ColumnaColSucu As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Sucu
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Sucu = value
        End Set
    End Property

    Public Property ColumnaColCSucu As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_CSucu
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_CSucu = value
        End Set
    End Property

    Public Property ColumnaColCNAp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_CNAp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_CNAp = value
        End Set
    End Property

    Public Property ColumnaColLineId As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_LineId
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_LineId = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Metodos"
    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        ColumnaColSucu = New ColumnaMatrixSBOEditText(Of String)("Col_Usua", True, "usua", Me)
        ColumnaColName = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "name", Me)
        ColumnaColCSucu = New ColumnaMatrixSBOEditText(Of String)("Col_CSucu", True, "csucu", Me)
        ColumnaColCNAp = New ColumnaMatrixSBOEditText(Of String)("Col_CNAp", True, "cnap", Me)
        ColumnaColLineId = New ColumnaMatrixSBOEditText(Of String)("Col_LineId", True, "lineid", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        ColumnaColSucu.AsignaBindingDataTable()
        ColumnaColName.AsignaBindingDataTable()
        ColumnaColCSucu.AsignaBindingDataTable()
        ColumnaColCNAp.AsignaBindingDataTable()
        ColumnaColLineId.AsignaBindingDataTable()
    End Sub
#End Region

End Class
