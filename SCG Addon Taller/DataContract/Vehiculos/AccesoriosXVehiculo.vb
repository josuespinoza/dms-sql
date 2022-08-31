Public Class AccesoriosxVehiculo


    Private _LogInst As Int32
    Public Property LogInst() As Int32
        Get
            Return _LogInst
        End Get
        Set(value As Int32)
            _LogInst = value
        End Set
    End Property


    Private _LineId As Int32
    Public Property LineId() As Int32
        Get
            Return _LineId
        End Get
        Set(value As Int32)
            _LineId = value
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


    Private _U_Acc As String
    Public Property U_Acc() As String
        Get
            Return _U_Acc
        End Get
        Set(value As String)
            _U_Acc = value
        End Set
    End Property


    Private _U_N_Acc As String
    Public Property U_N_Acc() As String
        Get
            Return _U_N_Acc
        End Get
        Set(value As String)
            _U_N_Acc = value
        End Set
    End Property


    Private _U_Tipo As String
    Public Property U_Tipo() As String
        Get
            Return _U_Tipo
        End Get
        Set(value As String)
            _U_Tipo = value
        End Set
    End Property

End Class
