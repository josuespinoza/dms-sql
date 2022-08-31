Public Class BonosXVehiculo

    Private _LineId As Int32
    Public Property LineId() As Int32
        Get
            Return _LineId
        End Get
        Set(value As Int32)
            _LineId = value
        End Set
    End Property


    Private _LogInst As Int32
    Public Property LogInst() As Int32
        Get
            Return _LogInst
        End Get
        Set(value As Int32)
            _LogInst = value
        End Set
    End Property


    Private _U_Monto As Double
    Public Property U_Monto() As Double
        Get
            Return _U_Monto
        End Get
        Set(value As Double)
            _U_Monto = value
        End Set
    End Property


    Private _Code As String
    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(value As String)
            _Code = value
        End Set
    End Property


    Private _U_Bono As String
    Public Property U_Bono() As String
        Get
            Return _U_Bono
        End Get
        Set(value As String)
            _U_Bono = value
        End Set
    End Property


End Class
