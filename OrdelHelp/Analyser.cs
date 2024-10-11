using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdelHelp
{
    public class Analyser
    {
        private readonly string[] _words;

        public Analyser(string[] contentLines)
        {
            _words = contentLines;
        }

        public Analyser(string content)
        {
            _words = content.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (int i = 0; i < _words.Length; i++)
            {
                _words[i] = _words[i].Trim().ToLower();
            }
        }


        public int Count => _words.Length;

        // pattern example: _y___ b....cb zq
        // we know the position of y and the word contains b which is not in pos 0 and 4 and also b that is not in position 4
        public string[] GetCandidates(string patternString)
        {
            var patternTokens = patternString.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var pattern = patternTokens[0].Trim();
            var positive = patternTokens[1].Trim();
            var negative = patternTokens[2].Trim();

            // temp hack - if there are duplicate chars we risk the first being green or yellow
            // and the second black. We just remove it from negatives for now
            negative = new string(negative.Except(positive).ToArray());

            // the positive part - chars that exsist but in the wrong place
            var dotPositions = positive
                .Select((c,index) => (c,index))
                .Where(pair => pair.c == '.')
                .Select(pair => pair.index)
                .ToArray();

            List<char[]> exsistingCharsInWrongPositions = new List<char[]>();
            char? previous = null;
            List<char>? currentList = null;
            for (int i = 0; i < positive.Length; i++)
            {
                var currentChar = positive[i];
                if (currentChar == '.')
                {
                    // last item - do we already have a list?
                    exsistingCharsInWrongPositions.Add(currentList?.ToArray() ?? Array.Empty<char>());
                    currentList = null;
                }
                else
                {
                    currentList = currentList ?? new List<char>();
                    currentList.Add(currentChar);
                }
            }

            if(currentList is not null)
            {
                exsistingCharsInWrongPositions.Add(currentList.ToArray());
            }

            var requiredCharacters = new HashSet<char>(exsistingCharsInWrongPositions.SelectMany(c => c));

            if (negative.Length == 1 && negative[0] == '-') negative = string.Empty;

            var wordLength = pattern.Length;
            var wildcardIndices = pattern.Select((c, index) => (c, index)).Where(tupel => tupel.c == '_').Select(tuple => tuple.index).ToArray();
            var trueIndices = pattern.Select((c, index) => (c, index)).Where(tupel => char.IsAsciiLetter(tupel.c)).Select(tuple => tuple.index).ToArray();

            List<string> candidates = new();
            for (int i = 0; i < _words.Length; i++)
            {
                var word = _words[i];

                // exclude on length
                if (word.Length != wordLength)
                    goto nextWordPlease;

                // skip if the does not match the char from known positions
                for (int trueIndiceIndex = 0; trueIndiceIndex < trueIndices.Length; trueIndiceIndex++)
                {
                    var currentExpectedIndex = trueIndices[trueIndiceIndex];
                    var currentExpectedChar = pattern[currentExpectedIndex];

                    if (word[currentExpectedIndex] == currentExpectedChar)
                    {
                        continue;
                    }
                    else
                    {
                        goto nextWordPlease;
                    }
                }

                // skip if word has chars from list of negatives
                for (int negativeIndex = 0; negativeIndex < negative.Length; negativeIndex++)
                {
                    if (word.Contains(negative[negativeIndex]))
                        goto nextWordPlease;
                }

                // skip if there are chars in wrong positions that are not in word at all
                var charsThatShouldBeContained = exsistingCharsInWrongPositions.SelectMany(arr => arr).ToArray()!;
                for (int charThatShouldBeContainedIndex = 0; charThatShouldBeContainedIndex < charsThatShouldBeContained.Length; charThatShouldBeContainedIndex++)
                {
                    var charThatShouldBeContained = charsThatShouldBeContained[charThatShouldBeContainedIndex];
                    if(word.Contains(charThatShouldBeContained) == false)
                    {
                        goto nextWordPlease;
                    }
                }
                
                
                // exclude on word missing chars from positives
                for (int positionInWord = 0; positionInWord < word.Length; positionInWord++)
                {
                    var currentCharacter = word[positionInWord];
                    var currentCollection = exsistingCharsInWrongPositions[positionInWord];


                    if(currentCollection.Contains(currentCharacter))
                    {
                        goto nextWordPlease;
                    }
                }

                // exclude words on chars that are correct but known to be in the wrong position
                for (int positionInWord = 0; positionInWord < word.Length; positionInWord++)
                {
                    var characterInCurrentWord = word[positionInWord];
                    var currentPositionCollection = exsistingCharsInWrongPositions[positionInWord];
                    for (int positionCollectionIndex = 0; positionCollectionIndex < currentPositionCollection.Length; positionCollectionIndex++)
                    {
                        if (characterInCurrentWord == currentPositionCollection[positionCollectionIndex])
                            goto nextWordPlease;
                    }
                }

                candidates.Add(word);


            // goto label at end of outer loop
            nextWordPlease:
                  _ = 1;
            }

            return candidates.ToArray();
        }
    }
}
