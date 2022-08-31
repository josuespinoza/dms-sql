Imports System

<Serializable()> _
Public Class DimensionesContables
    Public Property Sucursal() As String
        Get
            Return strSucursal
        End Get
        Set(ByVal value As String)
            strSucursal = value
        End Set
    End Property
    Private strSucursal As String

    Public Property CodigoMarca() As String
        Get
            Return strCodigoMarca
        End Get
        Set(ByVal value As String)
            strCodigoMarca = value
        End Set
    End Property
    Private strCodigoMarca As String

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

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean
End Class
