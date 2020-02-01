using Microsoft.Xna.Framework;
using System;
using Xunit;

namespace Tetrominoes.Tests
{
    public class MatchControllerTests
    {
        /*
         * OOOO |
         * X... V
         */
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void given_offset_move_down_should_lock_piece(int offset)
        {
            using var context = new TestContext(TetrominoPiece.Straight);

            context.Match.Grid[4 + offset, 3] = TetrominoPiece.J;

            var expected = context.Match.Tetrominoes.Current.Position;

            context.Match.Controller.Move.Down();

            var actual = context.Match.Tetrominoes.Current.Position;

            Assert.Equal(expected, actual);
        }

        /*
         * <--
         * x O
         *  OO
         * x O
         */
        [Fact]
        public void given_t_adapter_pattern_move_left_should_move_piece()
        {
            using var context = new TestContext(TetrominoPiece.T);

            context.Match.Controller.Rotate.Left();

            context.Match.Grid[3, 1] = TetrominoPiece.J;
            context.Match.Grid[3, 3] = TetrominoPiece.J;

            var expected = context.Match.Tetrominoes.Current.Position + new Vector2(-1, 0);

            context.Match.Controller.Move.Left();

            var actual = context.Match.Tetrominoes.Current.Position;

            Assert.Equal(expected, actual);
        }

        class TestContext : IDisposable
        {
            public Match Match { get; }
            public TestContext(TetrominoPiece piece)
            {
                var calculator = new PredictableMatchCalculator(
                    piece, piece, piece
                );
                Match = new Match(
                    calculator,
                    new MockOptionService()
                );
            }

            public void Dispose()
            {
                Match.Dispose();
            }
        }
    }
}
