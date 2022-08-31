Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Public Class MatrizAsignaMultiple : Inherits MatrixSBO

#Region "Declaraciones"

    Private _columnaCol_ID As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Status As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Stage As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Asignment As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDEmpAsign As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_NoOrden As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Dur As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDActOrd As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_NoCot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDFase As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_DesFase As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_LineNum As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaColID As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ID
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ID = value
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

    Public Property ColumnaColName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Name
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Name = value
        End Set
    End Property

    Public Property ColumnaColStatus As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Status
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Status = value
        End Set
    End Property

    Public Property ColumnaColStage As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Stage
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Stage = value
        End Set
    End Property

    Public Property ColumnaColAsignment As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Asignment
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Asignment = value
        End Set
    End Property

    Public Property ColumnaColIdEmpAsig As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDEmpAsign
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDEmpAsign = value
        End Set
    End Property

    Public Property ColumnaColDur As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Dur
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Dur = value
        End Set
    End Property

    Public Property ColumnaColIDActOrd As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDActOrd
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDActOrd = value
        End Set
    End Property

    Public Property ColumnaColNoOrden As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_NoOrden
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_NoOrden = value
        End Set
    End Property

    Public Property ColumnaColNoCot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_NoCot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_NoCot = value
        End Set
    End Property

    Public Property ColumnaColIDFase As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDFase
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDFase = value
        End Set
    End Property

    Public Property ColumnaColDesFase As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DesFase
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DesFase = value
        End Set
    End Property

    Public Property ColumnaColLineNum As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_LineNum
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_LineNum = value
        End Set
    End Property



#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub CreaColumnas()
        ColumnaColID = New ColumnaMatrixSBOEditText(Of String)("col_code", True, "col_code", Me)
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("col_sele", True, "col_sele", Me)
        ColumnaColName = New ColumnaMatrixSBOEditText(Of String)("col_desc", True, "col_desc", Me)
        ColumnaColStatus = New ColumnaMatrixSBOEditText(Of String)("col_esta", True, "col_esta", Me)
        ColumnaColStage = New ColumnaMatrixSBOEditText(Of String)("col_fase", True, "col_fase", Me)
        ColumnaColAsignment = New ColumnaMatrixSBOEditText(Of String)("col_asig", True, "col_asig", Me)
        ColumnaColDur = New ColumnaMatrixSBOEditText(Of String)("col_dura", True, "col_dura", Me)
        ColumnaColIDActOrd = New ColumnaMatrixSBOEditText(Of String)("col_idac", True, "col_idac", Me)
        ColumnaColIdEmpAsig = New ColumnaMatrixSBOEditText(Of String)("col_IDEmpA", True, "col_IDEmpA", Me)
        ColumnaColNoOrden = New ColumnaMatrixSBOEditText(Of String)("col_NoOrd", True, "col_NoOrd", Me)
        ColumnaColNoCot = New ColumnaMatrixSBOEditText(Of String)("col_NoCot", True, "col_NoCot", Me)
        ColumnaColIDFase = New ColumnaMatrixSBOEditText(Of String)("col_idfa", True, "col_idfa", Me)
        ColumnaColDesFase = New ColumnaMatrixSBOEditText(Of String)("col_desfa", True, "col_desfa", Me)
        ColumnaColLineNum = New ColumnaMatrixSBOEditText(Of String)("col_LnNum", True, "col_LnNum", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaColID.AsignaBindingDataTable()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColName.AsignaBindingDataTable()
        ColumnaColStatus.AsignaBindingDataTable()
        ColumnaColStage.AsignaBindingDataTable()
        ColumnaColAsignment.AsignaBindingDataTable()
        ColumnaColIdEmpAsig.AsignaBindingDataTable()
        ColumnaColNoOrden.AsignaBindingDataTable()
        ColumnaColDur.AsignaBindingDataTable()
        ColumnaColIDActOrd.AsignaBindingDataTable()
        ColumnaColIDFase.AsignaBindingDataTable()
        ColumnaColDesFase.AsignaBindingDataTable()
        ColumnaColNoCot.AsignaBindingDataTable()
        ColumnaColLineNum.AsignaBindingDataTable()
    End Sub

#End Region

End Class