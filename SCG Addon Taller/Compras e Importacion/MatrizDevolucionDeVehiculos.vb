Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizDevolucionDeVehiculos : Inherits MatrixSBO

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
    Private _columnaAsD As ColumnaMatrixSBOEditText(Of String)
    Private _columnaIdV As ColumnaMatrixSBOEditText(Of String)
    Private _columnaGR As ColumnaMatrixSBOEditText(Of String)


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
    Public Property ColumnaAsD As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaAsD
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaAsD = value
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
    Public Property ColumnaGR As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaGR
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaGR = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        ' ColumnaSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "", Me)
        ColumnaRec = New ColumnaMatrixSBOEditText(Of String)("col_Rece", True, "U_Num_Recepcion", Me)
        ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Pedi", True, "U_Num_Pedido", Me)
        ColumnaUni = New ColumnaMatrixSBOEditText(Of String)("col_Unid", True, "U_Cod_Unid", Me)
        ColumnaMar = New ColumnaMatrixSBOEditText(Of String)("col_Marc", True, "U_Desc_Marca", Me)
        ColumnaEst = New ColumnaMatrixSBOEditText(Of String)("col_Esti", True, "U_Desc_Estilo", Me)
        ColumnaMod = New ColumnaMatrixSBOEditText(Of String)("col_Mode", True, "U_Desc_Modelo", Me)
        ColumnaVin = New ColumnaMatrixSBOEditText(Of String)("col_Vin", True, "U_Num_VIN", Me)
        ColumnaMot = New ColumnaMatrixSBOEditText(Of String)("col_Mont", True, "U_Num_Motor", Me)
        ColumnaTip = New ColumnaMatrixSBOEditText(Of String)("col_Tipo", True, "U_Cod_Tipo_Inv", Me)
        ColumnaMon = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mont", True, "U_Monto_As", Me)
        ColumnaCur = New ColumnaMatrixSBOEditText(Of String)("col_Mone", True, "U_Moneda", Me)
        ColumnaTC = New ColumnaMatrixSBOEditText(Of Decimal)("col_TC", True, "U_Doc_Rate", Me)
        ColumnaAsi = New ColumnaMatrixSBOEditText(Of String)("col_Asie", True, "U_Num_Asiento", Me)
        ColumnaAsD = New ColumnaMatrixSBOEditText(Of String)("col_AsDe", True, "U_Num_As_Dev", Me)
        ColumnaIdV = New ColumnaMatrixSBOEditText(Of String)("col_Id", True, "U_Id_Veh", Me)
        ColumnaGR = New ColumnaMatrixSBOEditText(Of String)("col_GR", True, "U_Num_GR", Me)



    End Sub

    Public Overrides Sub LigaColumnas()

    End Sub
End Class
