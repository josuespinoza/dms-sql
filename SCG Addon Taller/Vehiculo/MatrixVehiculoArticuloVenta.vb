Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixVehiculoArticuloVenta
    : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)

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

    Public Property ColumnaCol_Name As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Name
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Name = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

    Public Overrides Sub CreaColumnas()

        ColumnaCol_Code = New ColumnaMatrixSBOEditText(Of String)("Col_Code", True, "code", Me)
        ColumnaCol_Name = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "name", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaCol_Code.AsignaBindingDataTable()
        ColumnaCol_Name.AsignaBindingDataTable()

    End Sub

End Class
