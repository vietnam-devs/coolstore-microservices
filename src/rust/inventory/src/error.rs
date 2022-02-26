use axum::{http::StatusCode, Json};
use serde_json::{json, Value};
use thiserror::Error;

pub enum Error {
    #[error(transparent)]
    TokioRecvError(#[from] tokio::sync::oneshot::error::RecvError),
    #[error(transparent)]
    AxumTypedHeaderError(#[from] axum::extract::rejection::TypedHeaderRejection),
    #[error(transparent)]
    AxumExtensionError(#[from] axum::extract::rejection::ExtensionRejection),
    #[error(transparent)]
    ValidationError(#[from] validator::ValidationErrors),
}

pub type Result<T> = std::result::Result<T, Error>;

pub type ApiError = (StatusCode, Json<Value>);
pub type ApiResult<T> = std::result::Result<T, ApiError>;

impl From<Error> for ApiError {
    fn from(err: Error) -> Self {
        let status = match err {
            Error::ValidationError(_) => StatusCode::BAD_REQUEST,
            _ => StatusCode::INTERNAL_SERVER_ERROR,
        };
        let payload = json!({"message": err.to_string()});
        (status, Json(payload))
    }
}

