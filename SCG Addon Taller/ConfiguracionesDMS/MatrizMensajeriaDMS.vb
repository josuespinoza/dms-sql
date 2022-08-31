Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizMensajeriaDMS
    : Inherits MatrixSBO

#Region "Declaraciones"

    'columnas de la matriz
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IdRol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_LineId As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_DocEntry As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IdUsr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_EmpCode As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_UserName As ColumnaMatrixSBOEditText(Of String)

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

    Public Property ColumnaColUserName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_UserName
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_UserName = value
        End Set
    End Property

    Public Property ColumnaColIdRol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IdRol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IdRol = value
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

    Public Property ColumnaCol_Docentry As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DocEntry
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DocEntry = value
        End Set
    End Property

    Public Property ColumnaCol_UsrId As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IdUsr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IdUsr = value
        End Set
    End Property

    Public Property ColumnaCol_EmpCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_EmpCode
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_EmpCode = value
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

        ColumnaColName = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "Name", Me)
        ColumnaCol_Docentry = New ColumnaMatrixSBOEditText(Of String)("Col_DE", True, "DocEntry", Me)
        ColumnaCol_EmpCode = New ColumnaMatrixSBOEditText(Of String)("Col_EmpID", True, "EmpId", Me)
        ColumnaColLineId = New ColumnaMatrixSBOEditText(Of String)("Col_LN", True, "LineId", Me)
        ColumnaCol_UsrId = New ColumnaMatrixSBOEditText(Of String)("Col_UsrID", True, "UsrID", Me)
        ColumnaColIdRol = New ColumnaMatrixSBOEditText(Of String)("Col_Rol", True, "RolId", Me)
        ColumnaColUserName = New ColumnaMatrixSBOEditText(Of String)("Col_Usua", True, "UserName", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        ColumnaColName.AsignaBindingDataTable()
        ColumnaCol_Docentry.AsignaBindingDataTable()
        ColumnaCol_EmpCode.AsignaBindingDataTable()
        ColumnaColLineId.AsignaBindingDataTable()
        ColumnaCol_UsrId.AsignaBindingDataTable()
        ColumnaColIdRol.AsignaBindingDataTable()
        ColumnaColUserName.AsignaBindingDataTable()
    End Sub
#End Region

End Class
