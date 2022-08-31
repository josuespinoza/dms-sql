Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizLineasConfiguracionOTs : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaSeleccionar As ColumnaMatrixSBOCheckBox(Of String)

    Public Property ColumnaSeleccionar() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccionar
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccionar = value
        End Set
    End Property

    Private _columnaCode As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCode() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCode
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCode = value
        End Set
    End Property

    Private _columnaName As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaName() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaName
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaName = value
        End Set
    End Property


    Private _columnaValor As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaValor() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaValor
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaValor = value
        End Set
    End Property

    Private _columnaUsaDimAEM As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaUsaDimAEM() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaUsaDimAEM
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaUsaDimAEM = value
        End Set
    End Property

    Private _columnaUsaDimAFP As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaUsaDimAFP() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaUsaDimAFP
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaUsaDimAFP = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()



        _columnaCode = New ColumnaMatrixSBOEditText(Of String)("colCode", True, "Code", Me)
        _columnaName = New ColumnaMatrixSBOEditText(Of String)("colName", True, "Name", Me)
        _columnaValor = New ColumnaMatrixSBOEditText(Of String)("colValor", True, "U_UsaDim", Me)
        _columnaUsaDimAEM = New ColumnaMatrixSBOEditText(Of String)("colUDimAEM", True, "U_UsaDimAEM", Me)
        _columnaUsaDimAFP = New ColumnaMatrixSBOEditText(Of String)("colUDimAFP", True, "U_UsaDimAFP", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaCode.AsignaBindingDataTable()
        _columnaName.AsignaBindingDataTable()
        _columnaValor.AsignaBindingDataTable()
        _columnaUsaDimAEM.AsignaBindingDataTable()
        _columnaUsaDimAFP.AsignaBindingDataTable()

    End Sub





End Class
