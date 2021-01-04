using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Solutions.Year2020
{
    class Day25 : ASolution
    {
        const long INITIAL_SUBJECT_NUMBER = 7;

        public Day25() : base(25, 2020, "")
        {
            AssertPartOne();
        }

        protected override string SolvePartOne()
        {
            var (publicKeyCard, publicKeyDoor) = GetPublicKeys();
            var loopSizeCard = FindLoopSize(publicKeyCard, INITIAL_SUBJECT_NUMBER);
            return PerformLoop(publicKeyDoor, loopSizeCard).ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private int FindLoopSize(long publicKey, long subjectNumber)
        {
            var loops = 0;
            var currentValue = 1L;
            while(publicKey != currentValue)
            {
                currentValue = PerformLoop(currentValue, subjectNumber);
                loops++;
            }

            return loops;
        }

        /// <summary>
        /// Loop to find the Encryption key
        /// </summary>
        private long PerformLoop(long subjectNumber, int loopSize) =>
            Enumerable.Range(0, loopSize).Aggregate(1L, (v1, v2) => PerformLoop(v1, subjectNumber));

        private long PerformLoop(long value, long subjectNumber) => (value * subjectNumber) % 20201227L;

        private void AssertPartOne()
        {
            base.DebugInput = "5764801\n17807724";
            var (publicKeyCard, publicKeyDoor) = GetPublicKeys();

            var loopSizeCard = FindLoopSize(publicKeyCard, INITIAL_SUBJECT_NUMBER);
            Assert.AreEqual(8, loopSizeCard);
            var loopSizeDoor = FindLoopSize(publicKeyDoor, INITIAL_SUBJECT_NUMBER);
            Assert.AreEqual(11, loopSizeDoor);

            var encryptionKeyUsingCardLoopSize = PerformLoop(publicKeyDoor, loopSizeCard);
            Assert.AreEqual(14897079L, encryptionKeyUsingCardLoopSize);
            var encryptionKeyUsingDoorLoopSize = PerformLoop(publicKeyCard, loopSizeDoor);
            Assert.AreEqual(14897079L, encryptionKeyUsingDoorLoopSize);

            base.DebugInput = null;
        }

        private (long publicKeyCard, long publicKeyDoor) GetPublicKeys() =>
            (long.Parse(base.Input.SplitByNewline()[0]), long.Parse(base.Input.SplitByNewline()[1]));
    }
}
