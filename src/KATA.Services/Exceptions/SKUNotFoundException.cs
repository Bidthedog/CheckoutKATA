using System;

namespace KATA.Services.Exceptions {
    /// <summary>
    ///     Demonstrates a basic custom exception for service consumer handling
    /// </summary>
    public class SKUNotFoundException : Exception {
        public SKUNotFoundException() {
        }
    }
}