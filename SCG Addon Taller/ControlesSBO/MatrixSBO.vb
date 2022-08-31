Option Strict On
Option Explicit On

Imports System.Globalization
Imports SAPbouiCOM

Namespace ControlesSBO


    <CLSCompliant(False)> _
    Public Class MatrixSBO
        Inherits ControlSBO

        Protected _matrix As IMatrix

        Public Sub New(ByVal itemSBO As IItem, ByVal permitirBuscar As Boolean)
            MyBase.New(itemSBO, permitirBuscar)
        End Sub

        Protected Overrides Sub AsignaControlEspecifico()
            _matrix = DirectCast(ItemSBO.Specific, IMatrix)
        End Sub

        Public ReadOnly Property Matrix() As IMatrix
            Get
                Return _matrix
            End Get
        End Property

        Public Sub AsignaValorColumnaEditText(ByVal valor As String, ByVal nombreColumna As String, ByVal posicion As Integer)
            DirectCast(_matrix.Columns.Item(nombreColumna).Cells.Item(posicion).Specific, EditText).Value = valor
        End Sub

        Public Function ObtieneValorColumnaEditText(ByVal nombreColumna As String, ByVal posicion As Integer) As String
            Return DirectCast(_matrix.Columns.Item(nombreColumna).Cells.Item(posicion).Specific, EditText).Value
        End Function
    End Class
End Namespace