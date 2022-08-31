'Matriz de usuarios

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizUsuarios
    : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Id As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaCol_Id As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Id
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Id = value
        End Set
    End Property

    Public Property ColumnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
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

#Region "Metodos"
    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        ColumnaCol_Id = New ColumnaMatrixSBOEditText(Of String)("Col_Id", True, "id", Me)
        ColumnaCol_Cod = New ColumnaMatrixSBOEditText(Of String)("Col_Cod", True, "cod", Me)
        ColumnaCol_Name = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "name", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        ColumnaCol_Id.AsignaBindingDataTable()
        ColumnaCol_Cod.AsignaBindingDataTable()
        ColumnaCol_Name.AsignaBindingDataTable()
    End Sub
#End Region
    
End Class
