Imports System

<Serializable()> _
Public Class Asiento
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property CuentaDebito() As String
        Get
            Return strCuentaDebito
        End Get
        Set(ByVal value As String)
            strCuentaDebito = value
        End Set
    End Property
    Private strCuentaDebito As String

    Public Property CuentaCredito() As String
        Get
            Return strCuentaCredito
        End Get
        Set(ByVal value As String)
            strCuentaCredito = value
        End Set
    End Property
    Private strCuentaCredito As String

    Public Property CuentaDiferencia() As String
        Get
            Return strCuentaDiferencia
        End Get
        Set(ByVal value As String)
            strCuentaDiferencia = value
        End Set
    End Property
    Private strCuentaDiferencia As String

    Public Property Moneda() As String
        Get
            Return strMoneda
        End Get
        Set(ByVal value As String)
            strMoneda = value
        End Set
    End Property
    Private strMoneda As String

    Public Property Costo() As Decimal
        Get
            Return decCosto
        End Get
        Set(ByVal value As Decimal)
            decCosto = value
        End Set
    End Property
    Private decCosto As Decimal

    Public Property CostoDiferencia() As Decimal
        Get
            Return decCostoDiferencia
        End Get
        Set(ByVal value As Decimal)
            decCostoDiferencia = value
        End Set
    End Property
    Private decCostoDiferencia As Decimal

    Public Property CostingCode() As String
        Get
            Return strCostingCode
        End Get
        Set(ByVal value As String)
            strCostingCode = value
        End Set
    End Property
    Private strCostingCode As String

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property
    Private strCostingCode2 As String


    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property
    Private strCostingCode3 As String

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property
    Private strCostingCode4 As String

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property
    Private strCostingCode5 As String

    Public Property IDSucursal() As String
        Get
            Return strIDSucursal
        End Get
        Set(ByVal value As String)
            strIDSucursal = value
        End Set
    End Property
    Private strIDSucursal As String

    Public Property UsaDimensiones() As Boolean
        Get
            Return blnUsaDimensiones
        End Get
        Set(ByVal value As Boolean)
            blnUsaDimensiones = value
        End Set
    End Property
    Private blnUsaDimensiones As Boolean

    Public Property U_SCGD_Cod_Tran() As String
        Get
            Return strU_SCGD_Cod_Tran
        End Get
        Set(ByVal value As String)
            strU_SCGD_Cod_Tran = value
        End Set
    End Property
    Private strU_SCGD_Cod_Tran As String

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property Proyecto() As String
        Get
            Return strProyecto
        End Get
        Set(ByVal value As String)
            strProyecto = value
        End Set
    End Property
    Private strProyecto As String
End Class
