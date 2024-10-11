using OrdelHelp;

namespace OrdelHelpTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test_BasedOnLength3()
        {
            var content = @"
                house
                baz";

            var wl = new Analyser(content);

            var candidates = wl.GetCandidates("___ ... -");
            var candidate = Assert.Single(candidates);
            Assert.Equal("baz", candidate);
        }

        [Fact]
        public void Test_BasedOnLength5()
        {
            var content = @"
                house
                baz";

            var wl = new Analyser(content);

            var candidates = wl.GetCandidates("_____ ..... -");
            var candidate = Assert.Single(candidates);
            Assert.Equal("house", candidate);
        }

        [Fact]
        public void Test_ExcludeOnPositivesShouldBePresent_ResultExcluded()
        {
            var content = @"
                house
                esuoh";

            var wl = new Analyser(content);

            var candidates = wl.GetCandidates("hou__ ....e -");
            Assert.Empty(candidates);
        }

        [Fact]
        public void Test_ExcludeOnPositivesShouldBePresent_ResultIncluded()
        {
            var content = @"
                house
                baz";

            var wl = new Analyser(content);

            var candidates = wl.GetCandidates("hou__ e.e.e... -");
            var candidate = Assert.Single(candidates);
            Assert.Equal("house", candidate);
        }

        [Fact]
        public void Test_PositiveOnPositionIsIncluded()
        {
            var content = @"
                abcde
                edcab";

            var wl = new Analyser(content);

            var candidates = wl.GetCandidates("____e ..... -");
            var candidate = Assert.Single(candidates);
            Assert.Equal("abcde", candidate);
        }


        [Fact]
        public void Test_Concrete_001()
        {
            var input = "_____ ...i.l. houseanv";

            // we know that it should contain 'l'
            var content = @"
                trick
                drift
                blitz";
            var analyzer = new Analyser(content);
            var candidates = analyzer.GetCandidates(input);
            Assert.Single(candidates, "blitz");
        }

        [Fact]
        public void Test_Concrete_002()
        {
            var input = "m__k___ .a.n..ai... cotesgnd";

            // note that mankind contains two occurences of 'n'. the first is 
            // positive in wrong position the second in negative. This tells
            // us that there are only one n
            var content = "mankind \n milkman \n";

            var analyser = new Analyser(content);
            var candidates = analyser.GetCandidates(input);
            Assert.Single(candidates, "milkman");
        }
    }
}