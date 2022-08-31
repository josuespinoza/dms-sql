Public Class SalidaMercanciaLineas

    Private strItemCode As String
    Private strWarehouseCode As String
    Private dblQuantity As Double
    Private strAccountCode As String
    Private strProjectCode As String
    Private strCostingCode As String
    Private strCostingCode2 As String
    Private strCostingCode3 As String
    Private strCostingCode4 As String
    Private strCostingCode5 As String

    Public Property ItemCode As String
        Get
            Return strItemCode
        End Get
        Set(value As String)
            strItemCode = value
        End Set
    End Property

    Public Property WarehouseCode As String
        Get
            Return strWarehouseCode
        End Get
        Set(value As String)
            strWarehouseCode = value
        End Set
    End Property

    Public Property Quantity As Double
        Get
            Return dblQuantity
        End Get
        Set(value As Double)
            dblQuantity = value
        End Set
    End Property
    
    Public Property AccountCode As String
        Get
            Return strAccountCode
        End Get
        Set(value As String)
            strAccountCode = value
        End Set
    End Property

    Public Property ProjectCode As String
        Get
            Return strProjectCode
        End Get
        Set(value As String)
            strProjectCode = value
        End Set
    End Property

    Public Property CostingCode As String
        Get
            Return strCostingCode
        End Get
        Set(value As String)
            strCostingCode = value
        End Set
    End Property

    Public Property CostingCode2 As String
        Get
            Return strCostingCode2
        End Get
        Set(value As String)
            strCostingCode2 = value
        End Set
    End Property

    Public Property CostingCode3 As String
        Get
            Return strCostingCode3
        End Get
        Set(value As String)
            strCostingCode3 = value
        End Set
    End Property

    Public Property CostingCode4 As String
        Get
            Return strCostingCode4
        End Get
        Set(value As String)
            strCostingCode4 = value
        End Set
    End Property

    Public Property CostingCode5 As String
        Get
            Return strCostingCode5
        End Get
        Set(value As String)
            strCostingCode5 = value
        End Set
    End Property
End Class
