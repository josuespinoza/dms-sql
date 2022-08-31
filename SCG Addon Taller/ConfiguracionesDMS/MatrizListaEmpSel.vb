Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizListaEmpSel
    : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_EmpId As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_UserCode As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_UserID As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_sel As ColumnaMatrixSBOCheckBox(Of String)

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

    Public Property ColumnaColEmpId As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_EmpId
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_EmpId = value
        End Set
    End Property

    Public Property ColumnaColUserCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_UserCode
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_UserCode = value
        End Set
    End Property

    Public Property ColumnaColUserID As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_UserID
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_UserID = value
        End Set
    End Property

    Public Property ColumnaColSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_sel = value
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

        ColumnaColName = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "Col_Name", Me)
        ColumnaColUserID = New ColumnaMatrixSBOEditText(Of String)("Col_UCode", True, "Col_UCode", Me)
        ColumnaColUserCode = New ColumnaMatrixSBOEditText(Of String)("Col_UN", True, "Col_UN", Me)
        ColumnaColEmpId = New ColumnaMatrixSBOEditText(Of String)("Col_EmId", True, "Col_EmId", Me)
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("col_sele", True, "col_sele", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        ColumnaColName.AsignaBindingDataTable()
        ColumnaColUserID.AsignaBindingDataTable()
        ColumnaColUserCode.AsignaBindingDataTable()
        ColumnaColEmpId.AsignaBindingDataTable()
        'ColumnaColSel.AsignaBindingDataTable()
    End Sub
#End Region

End Class
