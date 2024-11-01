namespace Clemency.Grid {
    public class GridObject<T> {
        GridSystem2D<GridObject<T>> _grid;
        int _x;
        int _y;
        T _value;

        public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y) {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void SetValue(T value) {
            _value = value;
        }

        public T GetValue() => _value;
    }
}