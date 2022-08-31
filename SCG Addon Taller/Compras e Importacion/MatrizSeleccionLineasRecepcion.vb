Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Public Class MatrizSeleccionLineasRecepcion : Inherits MatrixSBO

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
    Private _columnaIdV As ColumnaMatrixSBOEditText(Of String)

    Private _columnaCMa As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCEs As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCmo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaLin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol As ColumnaMatrixSBOEditText(Of String)

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
    Public Property ColumnaIdV As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaIdV
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaIdV = value
        End Set
    End Property

    Public Property ColumnaCodMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCMa
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCMa = value
        End Set
    End Property
    Public Property ColumnaCodEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCEs
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCEs = value
        End Set
    End Property
    Public Property ColumnaCodMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCmo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCmo = value
        End Set
    End Property

    Public Property ColumnaAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaAno
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaAno = value
        End Set
    End Property
    Public Property ColumnaArticulo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaArt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaArt = value
        End Set
    End Property
    Public Property ColumnaLineRef As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaLin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaLin = value
        End Set
    End Property
    Public Property ColumnaColor As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()

        ' ColumnaSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "", Me)
        ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Pedi", True, "pedi", Me)
        ColumnaRec = New ColumnaMatrixSBOEditText(Of String)("col_Rece", True, "rece", Me)
        ColumnaUni = New ColumnaMatrixSBOEditText(Of String)("col_Unid", True, "unid", Me)
        ColumnaMar = New ColumnaMatrixSBOEditText(Of String)("col_Marc", True, "marc", Me)
        ColumnaEst = New ColumnaMatrixSBOEditText(Of String)("col_Esti", True, "esti", Me)
        ColumnaMod = New ColumnaMatrixSBOEditText(Of String)("col_Mode", True, "mode", Me)
        ColumnaVin = New ColumnaMatrixSBOEditText(Of String)("col_Vin", True, "vin", Me)
        ColumnaMot = New ColumnaMatrixSBOEditText(Of String)("col_Moto", True, "moto", Me)
        ColumnaTip = New ColumnaMatrixSBOEditText(Of String)("col_Tipo", True, "tipo", Me)
        ColumnaIdV = New ColumnaMatrixSBOEditText(Of String)("col_Id", True, "code", Me)

        ColumnaCodMar = New ColumnaMatrixSBOEditText(Of String)("col_cMar", True, "cMar", Me)
        ColumnaCodEst = New ColumnaMatrixSBOEditText(Of String)("col_cEst", True, "cEst", Me)
        ColumnaCodMod = New ColumnaMatrixSBOEditText(Of String)("col_cMod", True, "cMod", Me)
        ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "ano", Me)
        ColumnaLineRef = New ColumnaMatrixSBOEditText(Of String)("col_Line", True, "line", Me)
        ColumnaArticulo = New ColumnaMatrixSBOEditText(Of String)("col_Arti", True, "arti", Me)
        ColumnaColor = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "col", Me)

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
        ColumnaIdV.AsignaBindingDataTable()

        ColumnaCodMar.AsignaBindingDataTable()
        ColumnaCodEst.AsignaBindingDataTable()
        ColumnaCodMod.AsignaBindingDataTable()
        ColumnaAno.AsignaBindingDataTable()
        ColumnaLineRef.AsignaBindingDataTable()
        ColumnaArticulo.AsignaBindingDataTable()
        ColumnaColor.AsignaBindingDataTable()
    End Sub
End Class
