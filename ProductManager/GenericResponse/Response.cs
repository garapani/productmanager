namespace ProductManager.GenericResponse
{
    public class Response<T>
    {
        public Response() { }

        public Response(ResultCode resultCode, T data, string message = null) {
            this.ResultCode = resultCode;
            this.Data = data;
            this.Message = message;
        }

        public Response(string message = null) {
            this.Message = message;
            this.ResultCode = ResultCode.BusinessLogicError;
        }

        public ResultCode ResultCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }

    public enum ResultCode {
        Success = 0,
        ValidationError = 1,
        InternalServerError = 2,
        BusinessLogicError = 3,
    }
}
