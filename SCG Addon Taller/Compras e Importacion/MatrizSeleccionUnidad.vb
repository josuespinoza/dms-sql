Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSeleccionUnidad : Inherits MatrixSBO


    Private _columnaSel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaRec As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPed As ColumnaMatrixSBOEditText(Of String)
    Private _columnaUni As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaEst As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaVin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTip As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMon As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCur As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTC As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaAsi As ColumnaMatrixSBOEditText(Of String)
    Private _columnaSta As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCod As ColumnaMatrixSBOEditText(Of String)


    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub


    Public Property ColumnaSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSel = value
        End Set
    End Property
    Public Property ColumnaRec As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaRec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaRec = value
        End Set
    End Property
    Public Property ColumnaPed As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPed
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPed = value
        End Set
    End Property
    Public Property ColumnaUni As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaUni
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaUni = value
        End Set
    End Property
    Public Property ColumnaMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMar = value
        End Set
    End Property
    Public Property ColumnaEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaEst
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaEst = value
        End Set
    End Property
    Public Property ColumnaMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMod = value
        End Set
    End Property
    Public Property ColumnaVin As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaVin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaVin = value
        End Set
    End Property
    Public Property ColumnaMot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMot = value
        End Set
    End Property
    Public Property ColumnaTip As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaTip
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaTip = value
        End Set
    End Property
    Public Property ColumnaMon As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaMon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaMon = value
        End Set
    End Property
    Public Property ColumnaCur As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCur
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCur = value
        End Set
    End Property
    Public Property ColumnaTC As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaTC
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaTC = value
        End Set
    End Property
    Public Property ColumnaAsi As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaAsi
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaAsi = value
        End Set
    End Property
    Public Property ColumnaSta As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaSta
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaSta = value
        End Set
    End Property
    Public Property ColumnaCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCod = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        ColumnaRec = New ColumnaMatrixSBOEditText(Of String)("col_Rec", True, "rece", Me)
        ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Ped", True, "pedi", Me)
        ColumnaUni = New ColumnaMatrixSBOEditText(Of String)("col_Uni", True, "unid", Me)
        ColumnaMar = New ColumnaMatrixSBOEditText(Of String)("col_Mar", True, "marc", Me)
        ColumnaEst = New ColumnaMatrixSBOEditText(Of String)("col_Est", True, "esti", Me)
        ColumnaMod = New ColumnaMatrixSBOEditText(Of String)("col_Mod", True, "mode", Me)
        ColumnaVin = New ColumnaMatrixSBOEditText(Of String)("col_Vin", True, "vin", Me)
        ColumnaMot = New ColumnaMatrixSBOEditText(Of String)("col_Mot", True, "moto", Me)
        ColumnaTip = New ColumnaMatrixSBOEditText(Of String)("col_Tip", True, "tipo", Me)
        ColumnaMon = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mon", True, "mont", Me)
        ColumnaCur = New ColumnaMatrixSBOEditText(Of String)("col_Cur", True, "mone", Me)
        ColumnaTC = New ColumnaMatrixSBOEditText(Of Decimal)("col_TC", True, "rate", Me)
        ColumnaAsi = New ColumnaMatrixSBOEditText(Of String)("col_Asi", True, "asie", Me)
        ColumnaSta = New ColumnaMatrixSBOEditText(Of String)("col_Sta", True, "stat", Me)
        ColumnaCod = New ColumnaMatrixSBOEditText(Of String)("col_Cod", True, "code", Me)


    End Sub

    Public Overrides Sub LigaColumnas()


        ColumnaRec.AsignaBindingDataTable()
        ColumnaPed.AsignaBindingDataTable()
        ColumnaUni.AsignaBindingDataTable()
        ColumnaMar.AsignaBindingDataTable()
        ColumnaEst.AsignaBindingDataTable()
        ColumnaMod.AsignaBindingDataTable()
        ColumnaVin.AsignaBindingDataTable()
        ColumnaMot.AsignaBindingDataTable()
        ColumnaTip.AsignaBindingDataTable()
        ColumnaMon.AsignaBindingDataTable()
        ColumnaCur.AsignaBindingDataTable()
        ColumnaTC.AsignaBindingDataTable()
        ColumnaAsi.AsignaBindingDataTable()
        ColumnaSta.AsignaBindingDataTable()
        ColumnaCod.AsignaBindingDataTable()

    End Sub


End Class
